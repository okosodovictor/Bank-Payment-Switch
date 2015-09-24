using BankSwitch.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Mappings
{
   public class TransactionLogMap:ClassMap<TransactionLog>
    {
       public TransactionLogMap()
       {
           Id(x => x.Id);
           Map(x => x.CardPAN);
           Map(x => x.Amount);
           Map(x => x.Account1);
           Map(x => x.Account2);
           Map(x => x.MTI);
           Map(x => x.ResponseCode);
           Map(x => x.ResponseDescription);
           Map(x => x.Route);
           Map(x => x.Scheme);
           Map(x => x.SinkNode);
           Map(X => X.SourceNode);
           Map(x => x.TransactionType);
           Map(x => x.Fee);
           Map(x => x.Channel);
           Map(x => x.STAN);
           Map(x => x.TransactionDate);
           Map(x => x.DateCreated);
           Map(x => x.DateModified);
           Map(x => x.OriginalDataElement);
           Map(x => x.IsReversePending);
           Map(x => x.IsReversed);
           Map(x => x.Charge);
       }
    }
}
