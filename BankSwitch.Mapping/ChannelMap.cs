using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
   public class ChannelMap:ClassMap<Channel>
    {
       public ChannelMap()
       {
           Id(x => x.Id);
           Map(x => x.Name);
           Map(x => x.Code);
           Map(x => x.Description);
       }
    }
}
