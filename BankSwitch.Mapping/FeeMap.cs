using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
   public class FeeMap:ClassMap<Fee>
    {
       public FeeMap()
       {
           Id(x => x.Id);
           Map(x => x.Name);
           Map(x => x.FlatAmount);
           Map(x => x.PercentageOfTransaction);
           Map(x => x.Maximum);
           Map(x => x.Minimum);
       }
    }
}
