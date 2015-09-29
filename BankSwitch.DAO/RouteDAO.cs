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
   public class RouteDAO:DataRepository
    {
       public RouteDAO()
       {

       }

       public IList<Route> SearchRoute(string queryString, int pageIndex, int pageSize, out int totalCount)
       {
           var channels = _Session.QueryOver<Route>();
           if (!string.IsNullOrEmpty(queryString))
           {
               channels.Where(x => x.Name.IsInsensitiveLike(queryString, MatchMode.Anywhere)).List<Route>();
           }
           else
           {
               channels.List<Route>();
           }
           var result = channels.Skip(pageIndex).Take(pageSize);
           totalCount = result.RowCount();
           return result.List<Route>();
       }

       public IList<Route> RetrieveByName(string name, string cardPan, int start, int limit, out int total)
       {
           List<Route> result = new List<Route>();
           try
           {
               ICriteria criteria = _Session.CreateCriteria(typeof(Route));
               if (!string.IsNullOrEmpty(name))
               {
                   criteria.Add(Expression.Like("Name", name));
               }
               if (!string.IsNullOrEmpty(cardPan))
               {
                   criteria.Add(Expression.Like("CardPAN", cardPan));
               }
               ICriteria countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(Projections.RowCountInt64());
               ICriteria listCriteria = CriteriaTransformer.Clone(criteria).SetFirstResult(start).SetMaxResults(limit);
               listCriteria.AddOrder(Order.Desc("Id"));

               IList allResults = _Session.CreateMultiCriteria().Add(listCriteria).Add(countCriteria).List();

               foreach (var o in (IList)allResults[0])
               {
                   result.Add((Route)o);
               }

               total = Convert.ToInt32((long)((IList)allResults[1])[0]);

               return result;
           }
           catch (Exception)
           {

               throw;
           }
       }
       public IList<Route> GetAllRoute()
       {
         return _Session.QueryOver<Route>().List<Route>();
       }
      public Route GetByCarPAN(string cardPAN)
       {
           IList<Route> route = null;
           using (var session = DataAccess.OpenSession())
           {
               route = session.QueryOver<Route>().List<Route>();
           }
           var result = route.Where(x => x.CardPAN == cardPAN).SingleOrDefault();
           return result;
       }
    } 
}
