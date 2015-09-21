using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
   public class RouteMap:ClassMap<Route>
    {
       public RouteMap()
       {
           Id(x => x.Id);
           Map(x => x.Name);
           References<SinkNode>(x => x.SinkNode).Not.LazyLoad();
           Map(x => x.Description);
           Map(x => x.CardPAN);
       }
    }
}
