using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.DAO
{
   public class SinkNodeDAO:DataRepository
    {
       public SinkNodeDAO()
       {

       }

       public IList<SinkNode> GetSinkNodes(string name, string hostName, string iPAddress)
       {
           try
           {
               if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(hostName) || !string.IsNullOrEmpty(iPAddress))
               {
                   var query = _Session.QueryOver<SinkNode>().Where(x => x.Name.IsLike(name, MatchMode.Anywhere)
                           || x.HostName.IsLike(hostName, MatchMode.Anywhere)
                           || x.IPAddress.IsLike(iPAddress, MatchMode.Anywhere)
                       ).List<SinkNode>();

                   return query;
               }
               else
               {
                   return _Session.QueryOver<SinkNode>().List<SinkNode>();
               }
           }
           catch (Exception)
           {
               
               throw;
           }
       }

       public IList<SinkNode> GetAllSinkNodes()
       {
           try
           {
               var query = _Session.QueryOver<SinkNode>().List<SinkNode>();
               return query;
           }
           catch (Exception)
           {
               throw;
           }
       }
    }
}
