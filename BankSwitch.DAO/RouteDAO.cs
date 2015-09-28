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

       public IList<Route> RetrieveByName(string name)
       {
           if(string.IsNullOrEmpty(name))
           {
               return _Session.QueryOver<Route>().List<Route>();
           }
           else
           {
            return   _Session.QueryOver<Route>().Where(x => x.Name.IsLike(name, MatchMode.Anywhere)).List<Route>();
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
