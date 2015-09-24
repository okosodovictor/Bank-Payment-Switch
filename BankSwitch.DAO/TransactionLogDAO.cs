using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
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
       public IList<TransactionLog> GetAllTransactionLog(string cardPAN, string mti, string responseCode, DateTime? transactionDateFrom, DateTime? transactionDateTo, int start, int limit, out int total)
       {
           List<TransactionLog> result = new List<TransactionLog>();
          try
          {
           ICriteria criteria = _Session.CreateCriteria(typeof(TransactionLog));
               if(!string.IsNullOrEmpty(cardPAN))
               {
                   criteria.Add(Expression.Like("CardPAN", cardPAN));
               }
               if (!string.IsNullOrEmpty(mti))
               {
                   criteria.Add(Expression.Like("MTI", mti));
               }
               if (!string.IsNullOrEmpty(responseCode))
              {
                  criteria.Add(Expression.Like("ResponseCode", responseCode));
              }
               if (transactionDateFrom.HasValue && !transactionDateFrom.Value.Equals(DateTime.MinValue))
               {
                   criteria.Add(Expression.Ge("TransactionDate", transactionDateFrom.Value));
               }
              if(transactionDateTo.HasValue && !transactionDateTo.Value.Equals(DateTime.MinValue))
              {
               criteria.Add(Expression.Le("TransactionDate", transactionDateTo.Value.AddDays(1).AddSeconds(-1)));
             }
             ICriteria countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(Projections.RowCountInt64());
                ICriteria listCriteria = CriteriaTransformer.Clone(criteria).SetFirstResult(start).SetMaxResults(limit);
                listCriteria.AddOrder(Order.Desc("Id"));

              IList allResults = _Session.CreateMultiCriteria().Add(listCriteria).Add(countCriteria).List();

              foreach (var o in (IList)allResults[0])
              {
                  result.Add((TransactionLog)o);
              }

              total = Convert.ToInt32((long)((IList)allResults[1])[0]);

            }
            catch (Exception)
            {

                throw;
            }
            return result;
       }
    }
}
