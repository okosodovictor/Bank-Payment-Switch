using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.DAO
{
   public class SchemeDAO:DataRepository
    {
       public SchemeDAO()
       {

       }

       public object Create(Scheme model)
       {
           object result = null;
           try
           {
               using (ISession session = _Session.SessionFactory.OpenSession())
               {
                   using (_Session.BeginTransaction())
                   {
                       Scheme scheme = new Scheme
                       {
                           Name = model.Name,
                           Route = model.Route,
                           Description = model.Description,
                       };
                       //session.SaveOrUpdate(scheme);

                       IList<TransactionTypeChannelFee> tcf = model.TransactionTypeChannelFees;
                       scheme.TransactionTypeChannelFees = tcf;

                       foreach (var item in tcf)
                       {
                           item.Scheme = scheme;
                           //session.Save(item);
                       }
                    result = session.Save(scheme);
                       
                   }
               }
               return result;
           }
           catch (Exception)
           {
               _Session.Transaction.Rollback();
               throw;
           }
       }

       public IList<Scheme> Search(string queryString, int pageIndex, int pageSize, out int totalCount)
       {
           var query = _Session.QueryOver<Scheme>();

           if (string.IsNullOrEmpty(queryString))
           {
               totalCount = query.RowCount();
               var schemes = query.List<Scheme>();
               return schemes;
           }
           else
           {
               query.Where(x => x.Name.IsLike(queryString, MatchMode.Anywhere)).List<Scheme>();
           }

           var result = query.Skip(pageIndex).Take(pageSize);
           totalCount = result.RowCount();
           return result.List<Scheme>();
       }

       public object UpdateScheme(Scheme model)
       {
           object result = null;
           using (var session = DataAccess.OpenSession())
           {
               using (var trnx = session.BeginTransaction())
               {
                   try
                   {
                result = session.Merge(model);
                   trnx.Commit();
                   }
                   catch (Exception)
                   {
                       trnx.Rollback();
                       throw;
                   }
                   return result;
               }
           }
       }
    }
}
