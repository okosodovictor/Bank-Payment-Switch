using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
   public class TransactionTypeMap:ClassMap<TransactionType>
    {
       public TransactionTypeMap()
       {
           Id(x => x.Id);
           Map(x => x.Name);
           Map(x => x.Description);
           Map(x => x.Code);
           
       }
    }
}
