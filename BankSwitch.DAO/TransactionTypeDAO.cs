using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using NHibernate.Criterion;
using System;
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

      public IList<TransactionType> Search(string querystring, int pageIndex, int pageSize, out int totalCount)
      {
          var query = _Session.QueryOver<TransactionType>();
          if (!string.IsNullOrEmpty(querystring))
          {
              query.Where(x => x.Code.IsInsensitiveLike(querystring, MatchMode.Anywhere)
                  ||x.Name.IsInsensitiveLike(querystring, MatchMode.Anywhere));
          }
          else
          {
              query.List<TransactionType>();
          }

          var result = query.Skip(pageIndex).Take(pageSize).Future<TransactionType>();
          totalCount = query.RowCount();
          return result.ToList();
      }
    }
}
