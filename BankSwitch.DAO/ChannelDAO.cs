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
   public class ChannelDAO:DataRepository
    {
       public ChannelDAO()
       {

       }
       public IList<Channel> SearchByName(string querystring)
       {
           if (string.IsNullOrEmpty(querystring))
           {
               return new List<Channel>();
           }
           else
           {

           return _Session.QueryOver<Channel>().Where(x => x.Code.IsInsensitiveLike(querystring, MatchMode.Anywhere)
               || x.Name.IsInsensitiveLike(querystring, MatchMode.Anywhere)).List<Channel>();
           }
       }
       public IList<Channel> Search(string queryparam, int pageIndex, int pageSize, out int totalCount)
       {
         var channels= _Session.QueryOver<Channel>();
           if(!string.IsNullOrEmpty(queryparam))
           {
              channels.Where(x => x.Name.IsInsensitiveLike(queryparam, MatchMode.Anywhere)
                   || x.Code.IsInsensitiveLike(queryparam, MatchMode.Anywhere)).List<Channel>();
           }
           var result = channels.Skip(pageIndex).Take(pageSize);
           totalCount = result.RowCount();
           return result.List<Channel>();
       }
       public IList<Channel> GetAllChannel()
       {
           var channels = _Session.QueryOver<Channel>().List<Channel>();
           return channels;
       }
    }
}
