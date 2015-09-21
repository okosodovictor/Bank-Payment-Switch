using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
    public class TransactionTypeChannelFeeMap : ClassMap<TransactionTypeChannelFee>
    {
        public TransactionTypeChannelFeeMap()
        {
            Id(x => x.Id);
            References(x => x.TransactionType).Not.LazyLoad();
            References(x => x.Fee).Not.LazyLoad();
            References(x => x.Channel).Not.LazyLoad();
            References(x => x.Scheme);
        }
    }
}
