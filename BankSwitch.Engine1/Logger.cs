using BankSwitch.Core.Entities;
using BankSwitch.Engine.Utility;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trx.Messaging.Iso8583;

namespace BankSwitch.Engine1
{
   public class Logger
    {
       public static void LogTransaction(Iso8583Message incomingMessage, SourceNode sourceNode = null, Scheme scheme = null, Fee fee = null, bool needReversal = false)
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
               var channel = new ChannelManager().GetByCode(channelCode);
               if (channel != null)
               {
                   transactionLog.Channel = channel.Name;
               }
               var trnx = new TransactionTypeManager().GetByCode(transactionTypeCode);
               if (trnx != null)
               {
                   transactionLog.TransactionType = trnx.Name;
               }
               transactionLog.SourceNode = sourceNode.Name;
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
                   transactionLog.Scheme = scheme.Name;
                   transactionLog.Route = scheme.Route.Name;

                   transactionLog.SinkNode = scheme.Route.SinkNode.Name;
               }

               try
               {
                   string value = incomingMessage.Fields[28].Value.ToString();
                   decimal result = 0;
                   if (Decimal.TryParse(value, out result))
                   {
                       transactionLog.Charge = result;
                   }
               }
               catch (Exception) { }
               if (fee != null)
               {
                   transactionLog.Fee = fee.Name;
               }
               if (responseCode != null)
               {
                   transactionLog.ResponseCode = responseCode;
               }
               if (responseCode != null)
               {
                   transactionLog.ResponseDescription = MessageDefinition.GetResponseDescription(responseCode);
               }
               string acc1 = incomingMessage.Fields[102].Value.ToString();
               string acc2 = incomingMessage.Fields[103].Value.ToString();
               transactionLog.Account1 = acc1;
               transactionLog.Account2 = acc2;
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
               Console.ForegroundColor = ConsoleColor.Red;
           }
       }

       public static void Log(string msg)
       {
           System.Diagnostics.Trace.TraceWarning(msg);
           using (StreamWriter writer = new StreamWriter(@"C:\Projects\BankSwitch\Log\Switch.txt", true))
           {
               writer.WriteLine(msg + "" + "\t" + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt"), Environment.NewLine);
           }
           Console.WriteLine("\n" + msg + "" + "\t" + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt"), Environment.NewLine);
           Console.ForegroundColor = ConsoleColor.Green;
       }
    }
}
