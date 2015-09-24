using BankSwitch.Core.Entities;
using BankSwitch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Logic
{
   public class TransactionLogManager
    {
       private TransactionLogDAO _db;
           public TransactionLogManager()
           {
               _db = new TransactionLogDAO();
           }

           public bool AddTransactionLog(TransactionLog model)
           {
               bool result = false;
               object obj = _db.AddTransactionLog(model);
               if (obj != null)
               {
                   result = true;
               }
               return result;
           }

           public TransactionLog GetByOriginalDataElement(string originalDataElement, out string originalDataElement2)
           {
               originalDataElement = originalDataElement.Remove(0, 23);
               originalDataElement2 = originalDataElement;
               return new TransactionLogDAO().GetByOriginalDataElement(originalDataElement);
           }

           public IList<TransactionLog> GetAllThatNeedsReversal()
           {
               var query = _db.GetAll<TransactionLog>().Where(x => x.IsReversePending).ToList();
               return query;
           }

           public void Update(TransactionLog thisLog)
           {
               if (thisLog != null)
               {
                   _db.Update(thisLog);
               }
           }
           public IList<TransactionLog> GetAllTransactionLog(string cardPAN, string mti, string responseCode, DateTime? transactionDatefrom, DateTime? transactionDateTo, int start, int limit, out int total)
           {
               return _db.GetAllTransactionLog(cardPAN, mti, responseCode, transactionDatefrom, transactionDateTo, start, limit, out total);
           }
    }
}
