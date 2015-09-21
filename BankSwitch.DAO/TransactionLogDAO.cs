using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.DAO
{
   public class TransactionLogDAO:DataRepository
    {
       public TransactionLogDAO()
       {

       }

       public object AddTransactionLog(TransactionLog model)
       {
           object result = null;
           using (var session = DataAccess.OpenSession())
           {
               using (var transactn = session.BeginTransaction())
               {
                   try
                   {
                       result = session.Save(model);
                       transactn.Commit();
                   }
                   catch (Exception)
                   {
                       transactn.Rollback();
                       throw;
                   }
               }
           }
           return result;
       }

       public TransactionLog GetByOriginalDataElement(string originalDataElement)
       {
           TransactionLog trxLog = null;
           if (!string.IsNullOrEmpty(originalDataElement))
           {

                trxLog = _Session.QueryOver<TransactionLog>().Where(x => x.OriginalDataElement == originalDataElement).SingleOrDefault();
           }
           else
           {
               trxLog = new TransactionLog();
           }
           return trxLog;
       }
    }
}
