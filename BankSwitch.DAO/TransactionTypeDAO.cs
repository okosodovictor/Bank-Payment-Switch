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
  public  class TransactionTypeDAO:DataRepository
    {
      public TransactionTypeDAO()
      {

      }

      public IList<TransactionType> Search(string name, string code, int start, int limit, out int total)
      {
          List<TransactionType> result = new List<TransactionType>();
          try
          {
              ICriteria criteria = _Session.CreateCriteria(typeof(TransactionType));
              if (!string.IsNullOrEmpty(name))
              {
                  criteria.Add(Expression.Like("Name", name));
              }
              if (!string.IsNullOrEmpty(code))
              {
                  criteria.Add(Expression.Like("Code", code));
              }
              ICriteria countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(Projections.RowCountInt64());
              ICriteria listCriteria = CriteriaTransformer.Clone(criteria).SetFirstResult(start).SetMaxResults(limit);
              listCriteria.AddOrder(Order.Desc("Id"));

              IList allResults = _Session.CreateMultiCriteria().Add(listCriteria).Add(countCriteria).List();

              foreach (var o in (IList)allResults[0])
              {
                  result.Add((TransactionType)o);
              }

              total = Convert.ToInt32((long)((IList)allResults[1])[0]);

              return result;
          }
          catch (Exception)
          {

              throw;
          }
      }
    }
}
