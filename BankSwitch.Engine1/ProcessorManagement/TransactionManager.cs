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
using System.Globalization;
using BankSwitch.Engine1;

namespace BankSwitch.Engine.ProcessorMangement
{
   public class TransactionManager
    {
       public TransactionManager()
       {
           Logger.Log("Switch Has Started....>");
       }

       public Iso8583Message ValidateMessage(Iso8583Message originalMessage, int sourceID)
       {
           Logger.Log("\n Enter Validator");
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
           Logger.LogTransaction(originalMessage, sourceNode);

           //  Check if it is reversal message and do the needful
           if (originalMessage.MessageTypeIdentifier == 421)
           {
               Logger.Log("\n This is a reversal");
               bool conReversal;
               Iso8583Message reversalIsoMsg = GetReversalMessage(originalMessage, out conReversal);
               if (!conReversal)
               {
                   Logger.LogTransaction(reversalIsoMsg);
                   return reversalIsoMsg;
               }
               originalMessage = reversalIsoMsg;
               Logger.LogTransaction(originalMessage, sourceNode);
           }

           string theCardPan = originalMessage.Fields[2].Value.ToString();
           string tranasactionTypeCode = originalMessage.Fields[3].Value.ToString().Substring(0, 2);
           double amount = Convert.ToDouble(originalMessage.Fields[4].Value);
           string orgExpiry = originalMessage.Fields[14].Value.ToString();


           string code = originalMessage.Fields[123].ToString().Substring(13, 2);

           Channel channel = new ChannelManager().GetByCode(code);
           Fee fee = null;
          // string cardPAN = theCardPan.Substring(0, 6);
           Route theRoute = new RouteManager().GetRouteByCardPan(theCardPan.Substring(0, 6));
           TransactionType transactionType = new TransactionTypeManager().GetByCode(tranasactionTypeCode);
           Iso8583Message responseMessage;


           //check if card has expired
           DateTime cardExpiryDate = ParseExpiryDate(orgExpiry);

           if(cardExpiryDate < DateTime.Now)
           {
               responseMessage = SetReponseMessage(originalMessage, "54");         //Expired card
               Logger.LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           if (amount <= 0 && tranasactionTypeCode != "31")
           {
               responseMessage = SetReponseMessage(originalMessage, "13");         //Invalid amount
               Logger.LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           if (theRoute == null)
           {
               Logger.Log("Sink node is null.");
               responseMessage = SetReponseMessage(originalMessage, "15");         //No such issuer
               Logger.LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }
           SinkNode sinkNode = theRoute.SinkNode;
           if (sinkNode == null)
           {
               Logger.Log("Sink node is null.");
               responseMessage = SetReponseMessage(originalMessage, "91");  //Issuer inoperative
               Logger.LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }


           Logger.Log("Loading SourceNode Schemes");

           var theSchemes = sourceNode.Schemes;
           Scheme scheme = null;
           try
           {
               scheme = theSchemes.Where(x => x.Route.CardPAN == theCardPan.Substring(0,6)).SingleOrDefault();
           }
           catch (Exception ex)
           {
               Logger.Log("Error: \n" + ex.Message);
               responseMessage = SetReponseMessage(originalMessage, "31"); // Lazy load error : Set correct response code later
               Logger.LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

           if (scheme == null)
           {
               responseMessage = SetReponseMessage(originalMessage, "92"); // Route not allowed : Set correct response code later
               Logger.LogTransaction(responseMessage, sourceNode);
               return responseMessage;
           }

          // int panCount = sourceNode.Schemes.Count(x => x.Route == theRoute);
           Logger.Log("Scheme : " + scheme.Name + " Loaded");

           Logger.Log("Getting fee:");
           fee = GetFeeIfTransactionIsAllowed(transactionType, channel, scheme);
           if (fee == null)
           {
               responseMessage = SetReponseMessage(originalMessage, "58"); // Transaction type not allowed in this scheme
               Logger.LogTransaction(responseMessage, sourceNode, scheme);
               return responseMessage;
           }
           else
           {
               originalMessage = SetFee(originalMessage, CalculateFee(fee, amount));
           }
           bool needReversal = false;

           Iso8583Message msgFromFEP = ToFEP(originalMessage, sinkNode, out needReversal);

           Logger.LogTransaction(msgFromFEP, sourceNode, scheme, fee, needReversal);


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
                   Logger.Log("Iso message is null.");
                   return SetReponseMessage(msgToSend, "20");  //Invalid response
               }

               if (sinkNode == null)
               {
                   Logger.Log("Sink node is null.");
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
                       Logger.Log("Connection timeout.");
                       needReversal = true;
                       return SetReponseMessage(msgToSend, "68");  //Response received too late
                   }
                   if (request != null)
                   {
                       response = request.ResponseMessage;
                       //ResponseMessage = GetResponseMesage(response as Iso8583Message);
                   }
                   return response as Iso8583Message;
               }
               else
               {
                   Console.WriteLine("\n Could not connect to the Sink Node..");
                   Console.BackgroundColor = ConsoleColor.Red;
                   Logger.Log("\n Could not connect to the Sink Node.");
                   return SetReponseMessage(msgToSend, "91");
               }
           }
           catch (Exception ex)
           {
               Logger.Log("ERROR: " + ex.Message);
               return SetReponseMessage(msgToSend, "06");  //Error
           }
       }

       private Fee GetFeeIfTransactionIsAllowed(TransactionType transactionType, Channel channel, Scheme scheme)
       {
           try
           {
               foreach (var item in scheme.TransactionTypeChannelFees)
               {
                   if (channel != null)
                   {
                       if (item.TransactionType.Code == transactionType.Code && item.Channel.Code == channel.Code)
                       {
                           return item.Fee;
                       }
                   }
               }

               return null;
           }
           catch (Exception ex)
           {
               Logger.Log("Error: while checking if transaction is allowed \n" + ex.Message);
               return null;
           }
       }

       private Iso8583Message SetFee(Iso8583Message message, double feeAmount)
       {
           message.Fields.Add(28, feeAmount.ToString());
           return message;
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
               if (feeCharged < fee.Minimum)
               {
                   feeCharged = fee.Minimum;
               }
           }
           Console.WriteLine("feeCharged is: " + feeCharged + "\n");
           return feeCharged;
       }

       private DateTime ParseExpiryDate(string orgExpiry)
       {
           string year = orgExpiry.Substring(0, 2);
           string month = orgExpiry.Substring(2, 2);
           string expiry = month + "-" + "01-" + year;
           DateTime cardExpiryDate = DateTime.Now;

           if (DateTime.TryParse(expiry, out cardExpiryDate))
           {
               return cardExpiryDate;
           }
           return DateTime.Now;
       }

       private Iso8583Message GetReversalMessage(Iso8583Message isoMessage, out bool conReversal)
       {
           conReversal = true;
           string originalDataElement;
           try
           {
               originalDataElement = isoMessage.Fields[90].ToString();
           }
           catch (Exception)
           {
               Logger.Log("Original data element is empty");
               conReversal = false;
               SetReponseMessage(isoMessage, "12");
               isoMessage.MessageTypeIdentifier = 430;
               return isoMessage;
           }

           //originalDataElement = originalDataElement.Remove(0, 23);
           TransactionLog transactionLog = new TransactionLogManager().GetByOriginalDataElement(originalDataElement, out originalDataElement);
           Logger.Log("Original Data Element: " + originalDataElement);
           if (transactionLog == null)
           {
               Logger.Log("\n Transaction log not found");
               conReversal = false;
               SetReponseMessage(isoMessage, "25");
               isoMessage.MessageTypeIdentifier = 430;
               return isoMessage;
           }

           if (transactionLog.IsReversed)
           {
               Logger.Log("\n Transaction has already been reversed");
               conReversal = false;
               SetReponseMessage(isoMessage, "94");
               return isoMessage;
           }

           Logger.Log("\n Continue with reversal");
           isoMessage.Fields.Add(102, transactionLog.Account2);
           isoMessage.Fields.Add(103, transactionLog.Account1);

           return isoMessage;
       }

       private Iso8583Message SetReponseMessage(Iso8583Message isoMessage, string code)
       {
           isoMessage.SetResponseMessageTypeIdentifier();
           isoMessage.Fields.Add(39, code);
           return isoMessage;
       }

      
       public void DoAutoReversal()
       {
           var allLogsThatNeedsReversal = new TransactionLogManager().GetAllThatNeedsReversal();
           Iso8583Message msgFromFEP = null;
           bool needReversal = true;
           var allSinkNodes = new SinkNodeManager().GetAllSinkNode().ToDictionary(x => x.Name);
           var allSourceNodes = new SourceNodeManager().RetrieveAll().ToDictionary(x => x.Name);
           foreach (var thisLog in allLogsThatNeedsReversal)
           {
               if (!thisLog.IsReversed && thisLog.IsReversePending)
               {
                   Iso8583Message revMsg = BuildReversalIsoMessage(thisLog);
                   Logger.LogTransaction(revMsg);
                   if (thisLog.SinkNode != null)
                   {
                       var sinkNode = allSinkNodes[thisLog.SinkNode];
                       msgFromFEP = ToFEP(revMsg, sinkNode, out needReversal); //TOFEP should set needReversal to false

                       if (msgFromFEP.Fields[39].ToString() == "00")
                       {
                           Logger.LogTransaction(msgFromFEP, allSourceNodes[thisLog.SourceNode], null, null, needReversal);
                       }
                   }

                   if (!needReversal)
                   {
                       thisLog.IsReversePending = false;
                       thisLog.IsReversed = true;
                       new TransactionLogManager().Update(thisLog);
                       Logger.Log("Auto Reversal done for: " + thisLog.OriginalDataElement);
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
           Iso8583Message echoMessage = new Iso8583Message(421);
           try
           {
               echoMessage.Fields.Add(2, log.CardPAN);
               echoMessage.Fields.Add(3, string.Format("{0}2000", "01"));
               echoMessage.Fields.Add(4, log.Amount.ToString());
               DateTime transmissionDate = DateTime.Now;
               echoMessage.Fields.Add(7, string.Format("{0}{1}",
                   string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day),
                   string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                   transmissionDate.Minute, transmissionDate.Second)));
               echoMessage.Fields.Add(11, log.STAN);
               echoMessage.Fields.Add(12, string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                   transmissionDate.Minute, transmissionDate.Second));
               echoMessage.Fields.Add(13, string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day));

               //echoMsg.Fields.Add(15, DateTime.Today.ToString("yyMM"));
               echoMessage.Fields.Add(22, "123");
               echoMessage.Fields.Add(25, "12");
               echoMessage.Fields.Add(28, "C000000");
               echoMessage.Fields.Add(29, "C000000");
               //echoMsg.Fields.Add(30, "C00000000");
               echoMessage.Fields.Add(32, "100002");
               //echoMsg.Fields.Add(35, string.Format("{0}={1:yyMM}", cardPAN, xpiryDate));
               //echoMsg.Fields.Add(37, transactionLog.RetrivalRefNo);
               //echoMsg.Fields.Add(40, "101");
               echoMessage.Fields.Add(41, "1701230J");
               echoMessage.Fields.Add(42, "VIACARD");
               echoMessage.Fields.Add(43, "Yaba, Office, Lagos");
               echoMessage.Fields.Add(49, "566");
               echoMessage.Fields.Add(90, "00000000000000000000000" + log.OriginalDataElement);
               echoMessage.Fields.Add(102, log.Account1);
               echoMessage.Fields.Add(103, log.Account2);
               echoMessage.Fields.Add(123, "01".PadLeft(15, '0'));
               Message inner = new Message();
               inner.Fields.Add(2, "IMNODE_00426629");
               //inner.Fields.Add(3, "ATMsrc      PRUICCsnk   000119000119ICCGroup    ");
               //inner.Fields.Add(20, DateTime.Today.ToString("yyMMdd"));

               echoMessage.Fields.Add(127, inner);
           }
           catch (Exception)
           {
               Logger.Log("Problem building reversal message");
               return null;
           }
           return echoMessage;
       }
    }
}
