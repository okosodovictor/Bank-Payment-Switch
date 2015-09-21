using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
   public  class SchemeMap:ClassMap<Scheme>
    {
       public SchemeMap()
       {
           Id(x => x.Id);
           Map(x => x.Name);
           References(x => x.Route).Not.LazyLoad();
           Map(x => x.Description);
           HasMany(x => x.TransactionTypeChannelFees).Inverse().Cascade.All().Not.LazyLoad();
       }
    }
}
