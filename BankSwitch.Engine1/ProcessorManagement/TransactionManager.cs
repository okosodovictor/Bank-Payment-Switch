using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using Trx.Messaging.Iso8583;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using BankSwitch.Engine.Utility;
using Trx.Messaging;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Channels;
using System.Threading;

namespace BankSwitch.Engine.ProcessorMangement
{
   public class TransactionManager
    {
       public TransactionManager()
       {
           Log("Switch Has Started....>");
       }

       public Iso8583Message ValidateMessage(Iso8583Message originalMessage, int sourceID)
       {
           Log("\n Enter Validator");
           //get source Node.
           SourceNode sourceNode = new SourceNodeManager().GetByID(sourceID);
           
           DateTime transmissionDate = DateTime.UtcNow;
           //format TransactionDate
           string transactionDate = string.Format("{0}{1}",
                 string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day),
                 string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                 transmissionDate.Minute, transmissionDate.Second));
           //set Original Data Element
             string originalDataElement = string.Format("{0}{1}{2}", originalMessage.MessageTypeIdentifier.ToString(), originalMessage.Fields[11].ToString(), transactionDate);
           //add original data element to original message
           originalMessage.Fields.Add(90, originalDataElement);
           
           //Do Message Log
           LogTransaction(originalMessage, sourceNode);

           //  Check if it is reversal message and do the needful
           if(originalMessage.MessageTypeIdentifier==421)
           {
               Log("\n This is Reversal Message");

                bool conReversal;
                Iso8583Message reversalIsoMsg = GetReversalMessage(originalMessage, out conReversal);
           }

           string theCardPan = originalMessage.Fields[2].Value.ToString();
           string trnxTypeCode = originalMessage.Fields[3].Value.ToString().Substring(0, 2);
           double amount = Convert.ToDouble(originalMessage.Fields[4].Value);
           string orgExpiry = originalMessage.Fields[14].Value.ToString();



           Channel theChannel = new ChannelManager().GetByCode(originalMessage.Fields[123].ToString().Substring(13, 2));
           Fee feeObj = null;
           Route theRoute = new RouteManager().GetRouteByCardPan(theCardPan);
           TransactionType transactionType = new TransactionTypeManager().GetByCode(trnxTypeCode);
           Iso8583Message responseMessage;


           //check if card has expired
           DateTime cardExpiryDate = ParseExpiryDate(orgExpiry);
           if (cardExpiryDate < DateTime.Now)
           {
               responseMessage = SetReponseMessage(originalMessage, "54");         //Expired card
               LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           if (amount <= 0 && trnxTypeCode != "31")
           {
               responseMessage = SetReponseMessage(originalMessage, "13");         //Invalid amount
               LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           if (theRoute == null)
           {
               Log("Sink node is null.");
               responseMessage = SetReponseMessage(originalMessage, "15");         //No such issuer
               LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           SinkNode sinkNode = theRoute.SinkNode;

           if (sinkNode == null)
           {
               Log("Sink node is null.");
               responseMessage = SetReponseMessage(originalMessage, "91");  //Issuer inoperative
               LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }


           Log("Loading Node Schemes");

           var theSchemes = sourceNode.Schemes;
           Scheme thisScheme = null;

           try
           {
               thisScheme = theSchemes.Where(x => x.Route.CardPAN == theCardPan).SingleOrDefault();
           }
           catch (Exception ex)
           {
               Log("Error: \n" + ex.Message);
               responseMessage = SetReponseMessage(originalMessage, "31"); // Lazy load error : Set correct response code later
               LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           if (thisScheme == null)
           {
               responseMessage = SetReponseMessage(originalMessage, "92"); // Route not allowed : Set correct response code later
               LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           int panCount = sourceNode.Schemes.Count(x => x.Route == theRoute);
           Log("Scheme : " + thisScheme.Name + " Loaded");

           feeObj = IsTransactionAllowed(transactionType, theChannel, thisScheme);

           Log("\n Getting fee");
           if (feeObj == null)
           {
               responseMessage = SetReponseMessage(originalMessage, "58"); // Transaction type not allowed in this scheme
               LogTransaction(responseMessage, sourceNode, thisScheme);
               return responseMessage;
           }
           else
           {
               originalMessage = SetFee(originalMessage, CalculateFee(feeObj, amount));
           }
           bool needReversal = false;
           Iso8583Message msgFromFEP = ToFEP(originalMessage, sinkNode, out needReversal);
           LogTransaction(msgFromFEP, sourceNode, thisScheme, feeObj, needReversal);


           return msgFromFEP;
       }

       private Iso8583Message ToFEP(Iso8583Message msgToSend, SinkNode sinkNode, out bool needReversal)
       {
           Message response = null;
           string responseMsg = string.Empty;
           needReversal = false;
           try
           {
               if (msgToSend == null)
               {
                   Log("Iso message is null.");
                   return SetReponseMessage(msgToSend, "20");  //Invalid response
               }

               if (sinkNode == null)
               {
                   Log("Sink node is null.");
                   return SetReponseMessage(msgToSend, "91");  //Issuer inoperative
               }

               int maxNoRetries = 3;
               int serverTimeout = 60000;

               sinkNode.IsActive = true;
               ClientPeer _clientPeer = new ClientPeer(sinkNode.Name, new TwoBytesNboHeaderChannel(
                       new Iso8583Ascii1987BinaryBitmapMessageFormatter(), sinkNode.IPAddress,
                       Convert.ToInt16(sinkNode.Port)), new Trx.Messaging.BasicMessagesIdentifier(11));
               _clientPeer.Connect();
               Thread.Sleep(1800);

               int retries = 0;
               while (retries < maxNoRetries)
               {
                   if (_clientPeer.IsConnected)
                   {
                       break;
                   }
                   else
                   {
                       _clientPeer.Close();
                       retries++;
                       _clientPeer.Connect();
                   }
                   Thread.Sleep(2000);
               }

               PeerRequest request = null;
               if (_clientPeer.IsConnected)
               {
                   request = new PeerRequest(_clientPeer, msgToSend);
                   request.Send();
                   request.WaitResponse(serverTimeout);
                   //request.MarkAsExpired();   //uncomment to test timeout

                   if (request.Expired)
                   {
                       Log("Connection timeout.");
                       needReversal = true;
                       return SetReponseMessage(msgToSend, "68");  //Response received too late
                   }
                   if (request != null)
                   {
                       response = request.ResponseMessage;
                       //ResponseMessage = GetResponseMesage(response as Iso8583Message);
                   }

                   //LogTransaction(response as Iso8583Message);
                   //_clientPeer.Close();
                   //Thread.Sleep(6000);
                   return response as Iso8583Message;
               }
               else
               {
                   Log("\n Could not connect to the Sink Node.");
                   return SetReponseMessage(msgToSend, "91");  //Issuer inoperative
               }
           }
           catch (Exception ex)
           {
               Log("ERROR: " + ex.Message);
               return SetReponseMessage(msgToSend, "06");  //Error
           }
       }

       private Fee IsTransactionAllowed(TransactionType transactionType, Channel theChannel, Scheme thisScheme)
       {
           try
           {
               foreach (var item in thisScheme.TransactionTypeChannelFees)
               {
                   if (item.TransactionType.Code == transactionType.Code && item.Channel.Code == theChannel.Code)
                   {
                       return item.Fee;
                   }
               }

               return null;
           }
           catch (Exception ex)
           {
               Log("Error: while checking if transaction is allowed \n" + ex.Message);
               return null;
           }
       }

       private Iso8583Message SetFee(Iso8583Message msg, double feeAmount)
       {
           msg.Fields.Add(28, feeAmount.ToString());
           return msg;
       }

       private double CalculateFee(Fee fee, double amount)
       {
           double feeCharged = 0.0;
           if (fee.FlatAmount > 0)
           {
               feeCharged = Convert.ToDouble(fee.FlatAmount);
           }
           else
           {
               double perOfTransaction = (Convert.ToDouble(fee.PercentageOfTransaction) * 0.01);
               feeCharged = amount * perOfTransaction;
               if (feeCharged > fee.Maximum)
               {
                   feeCharged = fee.Maximum;
               }
               else if (feeCharged < fee.Minimum)
               {
                   feeCharged = fee.Minimum;
               }
               else { }
           }
           Console.WriteLine("feeCharged is: " + feeCharged + "\n");
           return feeCharged;
       }

       private DateTime ParseExpiryDate(string orgExpiry)
       {
           string year = orgExpiry.Substring(0, 2);
           string month = orgExpiry.Substring(2, 2);

           string expiry = "01-" + month + "-" + year;
           DateTime cardExpiryDate = DateTime.Now;

           if (DateTime.TryParse(expiry, out cardExpiryDate))
           {
               return cardExpiryDate;
           }
           return DateTime.Now;
       }

       private Iso8583Message GetReversalMessage(Iso8583Message isoMsg, out bool conReversal)
       {
           conReversal = true;
           string originalDataElement;
           try
           {
               originalDataElement = isoMsg.Fields[90].ToString();
           }
           catch (Exception)
           {
               Log("Original data element is empty");
               conReversal = false;
               SetReponseMessage(isoMsg, "12");
               isoMsg.MessageTypeIdentifier = 430;
               return isoMsg;
           }

           //originalDataElement = originalDataElement.Remove(0, 23);
           TransactionLog transactionLog = new TransactionLogManager().GetByOriginalDataElement(originalDataElement, out originalDataElement);
           Log("Original Data Element: " + originalDataElement);
           if (transactionLog == null)
           {
               Log("\n Transaction log not found");
               conReversal = false;
               SetReponseMessage(isoMsg, "25");
               isoMsg.MessageTypeIdentifier = 430;
               return isoMsg;
           }

           if (transactionLog.IsReversed)
           {
               Log("\n Transaction has already been reversed");
               conReversal = false;
               SetReponseMessage(isoMsg, "94");
               return isoMsg;
           }

           Log("\n Continue with reversal");
           isoMsg.Fields.Add(102, transactionLog.Account2);
           isoMsg.Fields.Add(103, transactionLog.Account1);

           return isoMsg;
       }

       private Iso8583Message SetReponseMessage(Iso8583Message isoMsg, string code)
       {
           isoMsg.SetResponseMessageTypeIdentifier();
           isoMsg.Fields.Add(39, code);
           return isoMsg;
       }

       private void LogTransaction(Iso8583Message incomingMessage, SourceNode sourceNode = null, Scheme scheme =null , Fee fee = null, bool needReversal = false)
       {
           var instCode = incomingMessage.Fields[2].ToString().Substring(0, 6);
           string transactionTypeCode = (incomingMessage.Fields[3].ToString().Substring(0, 2));
           string channelCode = incomingMessage.Fields[123].ToString().Substring(13, 2);
           string cardPan = incomingMessage.Fields[2].ToString();
           string response = string.Empty;
           string responseCode = string.Empty;
           DateTime transmissionDate = DateTime.UtcNow;

           TransactionLog transactionLog = new TransactionLog();
           try
           {
               transactionLog.MTI = incomingMessage.MessageTypeIdentifier.ToString();
               transactionLog.STAN = incomingMessage.Fields[11].ToString();
               transactionLog.Amount = Convert.ToDouble(incomingMessage.Fields[4].ToString());
               transactionLog.CardPAN = cardPan;
               transactionLog.Channel = new ChannelManager().GetByCode(channelCode);
               transactionLog.TransactionType = new TransactionTypeManager().GetByCode(transactionTypeCode);
               transactionLog.SourceNode = sourceNode;
               transactionLog.TransactionDate = transmissionDate;
               transactionLog.DateCreated = DateTime.Now;
               transactionLog.DateModified = DateTime.Now;

               string orDataElt = incomingMessage.Fields[90].ToString();
               int length = orDataElt.Length;
               transactionLog.OriginalDataElement = orDataElt.Length > 19 ? orDataElt.Remove(0, (length - 19)) : orDataElt;

               try
               {
                   responseCode = incomingMessage.Fields[39].Value.ToString();
               }
               catch (Exception) { }


               if (scheme != null)
               {
                   transactionLog.Scheme = scheme;
                   transactionLog.Route = scheme.Route;

                   transactionLog.SinkNode = scheme.Route.SinkNode;
               }

               try
               {
                   transactionLog.Charge = Convert.ToDecimal(incomingMessage.Fields[28].Value.ToString());
               }
               catch (Exception) { }
   
               transactionLog.Fee = fee;
               transactionLog.ResponseCode = responseCode;
               transactionLog.ResponseDescription = MessageDefinition.GetResponseDescription(responseCode);

               transactionLog.Account1 = incomingMessage.Fields[102].ToString();
               transactionLog.Account2 = incomingMessage.Fields[103].ToString();
               transactionLog.IsReversePending = needReversal;

               if (incomingMessage.MessageTypeIdentifier.ToString() == "430" && responseCode == "00")
               {
                   transactionLog.IsReversed = true;
                   // SetReversalStatus(incomingMsg, responseCode);  //here
               }

               if (new TransactionLogManager().AddTransactionLog(transactionLog))
               {
                   Log("Transaction log::: " + transactionLog.STAN + " " + transactionLog.TransactionDate);
               }
               else
               {
                   Log("Transaction log::: not successful");
               }
           }
           catch (Exception ex)
           {
               Log("Error occurred while logging transaction \n" + ex.Message);
           }
       }


       public void Log(string msg)
       {
           System.Diagnostics.Trace.TraceWarning(msg);
           using (StreamWriter writer = new StreamWriter(@"C:\Projects\BankSwitch\Log\Switch.txt", true))
           {
               writer.WriteLine(msg + "" +"\t"+ DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt"), Environment.NewLine);
           }
           Console.WriteLine("\n" + msg + "" +"\t"+ DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt"), Environment.NewLine);
       }

       public void DoAutoReversal()
       {
           var allLogThatNeedsReversal = new TransactionLogManager().GetAllThatNeedsReversal();
           Iso8583Message msgFromFEP = null;
           bool needReversal = true;

           foreach (var thisLog in allLogThatNeedsReversal)
           {
               if (!thisLog.IsReversed && thisLog.IsReversePending)
               {
                   Iso8583Message revMsg = BuildReversalIsoMessage(thisLog);
                   LogTransaction(revMsg);
                   if (thisLog.SinkNode != null)
                   {
                       var sinkNode = new SinkNodeManager().GetById(thisLog.SinkNode.Id);
                       msgFromFEP = ToFEP(revMsg, sinkNode, out needReversal); //TOFEP should set needReversal to false

                       if (msgFromFEP.Fields[39].ToString() == "00")
                       {
                           LogTransaction(msgFromFEP, thisLog.SourceNode, null, null, needReversal);
                       }
                   }

                   if (!needReversal)
                   {
                       thisLog.IsReversePending = false;
                       thisLog.IsReversed = true;
                       new TransactionLogManager().Update(thisLog);
                       Log("Auto Reversal done for: " + thisLog.OriginalDataElement);
                   }
               }
               if (thisLog.IsReversed)
               {
                   thisLog.IsReversePending = false;
                   thisLog.IsReversed = true;
                   new TransactionLogManager().Update(thisLog);
               }
           }
       }

       private Iso8583Message BuildReversalIsoMessage(TransactionLog log)
       {
           Iso8583Message echoMsg = new Iso8583Message(421);
           try
           {
               echoMsg.Fields.Add(2, log.CardPAN);
               echoMsg.Fields.Add(3, string.Format("{0}2000", "01"));
               echoMsg.Fields.Add(4, log.Amount.ToString());
               DateTime transmissionDate = DateTime.Now;
               echoMsg.Fields.Add(7, string.Format("{0}{1}",
                   string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day),
                   string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                   transmissionDate.Minute, transmissionDate.Second)));
               echoMsg.Fields.Add(11, log.STAN);
               echoMsg.Fields.Add(12, string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                   transmissionDate.Minute, transmissionDate.Second));
               echoMsg.Fields.Add(13, string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day));

               //echoMsg.Fields.Add(15, DateTime.Today.ToString("yyMM"));
               echoMsg.Fields.Add(22, "123");
               echoMsg.Fields.Add(25, "12");
               echoMsg.Fields.Add(28, "C000000");
               echoMsg.Fields.Add(29, "C000000");
               //echoMsg.Fields.Add(30, "C00000000");
               echoMsg.Fields.Add(32, "100002");
               //echoMsg.Fields.Add(35, string.Format("{0}={1:yyMM}", cardPAN, xpiryDate));
               //echoMsg.Fields.Add(37, transactionLog.RetrivalRefNo);
               //echoMsg.Fields.Add(40, "101");
               echoMsg.Fields.Add(41, "1701230J");
               echoMsg.Fields.Add(42, "VIACARD");
               echoMsg.Fields.Add(43, "IronBar, Lekki, Lagos");
               echoMsg.Fields.Add(49, "566");
               echoMsg.Fields.Add(90, "00000000000000000000000" + log.OriginalDataElement);
               echoMsg.Fields.Add(102, log.Account1);
               echoMsg.Fields.Add(103, log.Account2);
               echoMsg.Fields.Add(123, "01".PadLeft(15, '0'));
               Message inner = new Message();
               inner.Fields.Add(2, "IMNODE_00426629");
               //inner.Fields.Add(3, "ATMsrc      PRUICCsnk   000119000119ICCGroup    ");
               //inner.Fields.Add(20, DateTime.Today.ToString("yyMMdd"));

               echoMsg.Fields.Add(127, inner);
           }
           catch (Exception)
           {
               Log("Problem building reversal message");
               return null;
           }
           return echoMsg;
       }
    }
}
