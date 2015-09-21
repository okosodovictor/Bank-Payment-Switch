using BankSwitch.Core.Entities;
using BankSwitch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Logic
{
   public class ChannelManager
    {
       private ChannelDAO _db;
       public ChannelManager(ChannelDAO db)
       {
           _db = db;
       }
       public ChannelManager()
       {
           _db = new ChannelDAO();
       }

       public bool CreateChannel(Channel model)
       {
           var ch = _db.Get<Channel>().FirstOrDefault(x => x.Name == model.Name && x.Code == model.Code);
           if(ch!=null)
           {
               throw new Exception("This Channel Already Exist");
           }
           else
           {
               return _db.Add(new Channel
               {
                    Name=model.Name,
                    Code=model.Code,
                    Description= model.Description
               });
           }
       }
       public bool Edit(Channel model)
       {
           bool result = false;
           var ch = _db.Get<Channel>().FirstOrDefault(x => x.Name == model.Name && x.Code == model.Code);
           if(ch!=null)
           {
               ch.Name = model.Name;
               ch.Code = model.Code;
               ch.Description = model.Description;
              result =  _db.Update(ch);
           }
           return result;
       }

       public IList<Channel> SearchByName(string Name)
       {
           return _db.SearchByName(Name);
       }
       public  IList<Channel> Search(string querystring, int pageSize, int pageLimit, out int totalCount)
       {
           return _db.Search(querystring, pageSize, pageLimit, out totalCount);
       }
       public IList<Channel> GetAllChannel()
       {
           return _db.GetAllChannel();
       }
       public IList<Channel> RetreiveChannels(string name, string code)
       {
           if(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(code))
           {
               return _db.GetAll<Channel>().ToList();
           }
           else
           {
          return _db.GetAll<Channel>().Where(x => x.Name == name || x.Code == code).ToList();
           }
       }

       public Channel GetByCode(string channelCode)
       {
           var channel = _db.Get<Channel>().Where(x => x.Code == channelCode).FirstOrDefault();
           return channel;
       }
    }
}
