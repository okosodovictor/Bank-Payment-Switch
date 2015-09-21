using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Engine.Utility
{
   public class MessageDefinition
    {

       public static string GetMtiDescription(int mtiCode, string nodeType, out bool isValidMti)
       {
           isValidMti = true;
           try
           {
               string mtidec = nodeType == MessageCode.fep
                   ? MtiDescriptionFromFEP.First(x => x.Key == mtiCode).Value
                   : (nodeType == MessageCode.source ? MTIDescriptorFromSourceNode.First(x => x.Key == mtiCode).Value : "Unknown");
               isValidMti = true;
               return mtidec;
           }
           catch (Exception)
           {
               isValidMti = false;
               return "Invalid MTI";
           }

       }

        public static readonly Dictionary<string, string> ResponseDescriptions = new Dictionary<string, string>
           {
               {"00", "Approve by Financial Institution"},
               {"01", "Refer to card issuer"},
               {"02", "Refer to card issuer, special condition"},
               {"03", "Invalid Merchant"},
               {"04", "Pick-up card"},
               {"05", "Do Not Honor"},
               {"06", "Error"},
               {"07", "Pick-Up Card, Special Condition"},
               {"08", "Honor with Identification"},
               {"09", "Request in Progress"},
               {"10", "Approved by Financial Institution, Partial"},
               {"11", "Approved by Financial Institution, VIP"},
               {"12", "Invalid Transaction"},
               {"13", "Invalid Amount"},
               {"14", "Invalid Card Number"},
               {"15", "No Such issuer"},
               {"16", "Approved by Financial Institution, Update Track 3"},
               {"17", "Customer Cancellation"},
               {"18", "Customer Dispute"},
               {"19", "Re-enter Transaction"},
               {"20", "Invalid Response from Financial Institution"},
               {"21", "No Action Taken by Financial Institution"},
               {"22", "Suspected Malfunction"},
               {"23", "Unacceptable Transaction Fee"},
               {"24", "File Update not Supported"},
               {"25", "Unable to Locate Record"},
               {"26", "Duplicate Record"},
               {"27", "File Update Field Edit Error"},
               {"28", "File Update File Locked"},
               {"29", "File Update Failed"},
               {"30", "Format Error"},
               {"31", "Bank Not Supported"},
               {"32", "Completed Partially by Financial Institution"},
               {"33", "Expired Card, Pick-Up"},
               {"34", "Suspected Fraud, Pick-Up"},
               {"35", "Contact Acquirer, Pick-Up"},
               {"36", "Restricted Card, Pick-Up"},
               {"37", "Call Acquirer Security, Pick-Up"},
               {"38", "PIN Tries Exceeded, Pick-Up"},
               {"39", "No Credit Account"},
               {"40", "Function not Supported"},
               {"41", "Lost Card, Pick-Up"},
               {"42", "No Universal Account"},
               {"43", "Stolen Card, Pick-Up"},
               {"44", "No Investment Account"},
               {"51", "Insufficient Funds"},
               {"52", "No Check Account"},
               {"53", "No Savings Account"},
               {"54", "Expired Card"},
               {"55", "Incorrect PIN"},
               {"56", "No Card Record"},
               {"57", "Transaction not Permitted to Cardholder"},
               {"58", "Transaction not Permitted on Terminal"},
               {"59", "Suspected Fraud"},
               {"60", "Contact Acquirer"},
               {"61", "Exceeds Withdrawal Limit"},
               {"62", "Restricted Card"},
               {"63", "Security Violation"},
               {"64", "Original Amount Incorrect"},
               {"65", "Exceeds withdrawal frequency"},
               {"66", "Call Acquirer Security"},
               {"67", "Hard Capture"},
               {"68", "Response Received Too Late"},
               {"75", "PIN tries exceeded"},
               {"76", "Reserved for Future Postilion Use"},
               {"77", "Intervene, Bank Approval Required"},
               {"78", "Intervene, Bank Approval Required for Partial Amount"},
               {"90", "Cut-off in Progress"},
               {"91", "Issuer or Switch Inoperative"},
               {"92", "Routing Error"},
               {"93", "Violation of law"},
               {"94", "Duplicate Transaction"},
               {"95", "Reconcile Error"},
               {"96", "System Malfunction"},
               {"98", "Exceeds Cash Limit"},
           };


        private static readonly Dictionary<int, string> MTIDescriptorFromSourceNode = new Dictionary<int, string>
       {
           {100, "Authorization request"},
           {101, "(Repeat) Authorization request "},
           {200, "Financial request"},
           {201, "(Repeat) Financial request"},
           {420, "Reversal advice"},
           {421, "(Repeat) Reversal advice"},
        };

        public static Dictionary<int, string> MtiDescriptionFromFEP = new Dictionary<int, string>
       {
           {110, "Authorization request response"},
           {210, "Financial request response"},
           {430, "Reversal advice response"},
           {130, "Authorization advice Response"},
           {212, "Financial completion Response"},
           {230, "Financial Transaction Advice Response"},
           {610, "Admin Response"}
        };


        public static string GetResponseDescription(string responseCode)
        {
            try
            {
                return ResponseDescriptions.SingleOrDefault(x => x.Key.ToString() == responseCode).Value;
            }
            catch (Exception)
            {
                return "Unknown response code";
            }
        }
    }
}
