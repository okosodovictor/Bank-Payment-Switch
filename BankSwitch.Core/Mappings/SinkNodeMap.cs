using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
   public class SinkNodeMap:ClassMap<SinkNode>
    {
       public SinkNodeMap()
       {
           Id(x => x.Id);
           Map(x => x.Name);
           Map(x => x.HostName);
           Map(x => x.IPAddress);
           Map(x => x.Port);
           Map(x => x.IsActive);
       }
    }
}
