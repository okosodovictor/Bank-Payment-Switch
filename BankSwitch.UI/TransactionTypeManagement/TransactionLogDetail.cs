using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankSwitch.UI.TransactionTypeManagement
{
   public class TransactionLogDetail:EntityUI<TransactionLog>
    {
       public TransactionLogDetail()
       {

           UseFullView();
           WithTitle("View TransactionLog Details");
           AddSection()
               .IsFormGroup()
               .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.ResponseCode).AsSectionField<TextLabel>(),
                             Map(x => x.CardPAN).AsSectionField<TextLabel>(),
                             Map(x => x.Channel).AsSectionField<TextLabel>(),
                             Map(x => x.Amount).AsSectionField<TextLabel>(),
                             Map(x => x.Charge).AsSectionField<TextLabel>(),

                             Map(x => x.Account1).AsSectionField<TextLabel>(),
                             Map(x => x.Account2).AsSectionField<TextLabel>(),
                             Map(x => x.DateCreated).AsSectionField<TextLabel>(),
                             Map(x => x.DateModified).AsSectionField<TextLabel>(),
                             Map(x => x.Fee).AsSectionField<TextLabel>(),

                             Map(x => x.MTI).AsSectionField<TextLabel>(),
                             Map(x => x.IsReversed).AsSectionField<TextLabel>(),
                             Map(x => x.IsReversePending).AsSectionField<TextLabel>(),
                             Map(x => x.OriginalDataElement).AsSectionField<TextLabel>(),
                             Map(x => x.ResponseDescription).AsSectionField<TextLabel>(),

                             Map(x => x.Route).AsSectionField<TextLabel>(),
                             Map(x => x.Scheme).AsSectionField<TextLabel>(),
                             Map(x => x.SinkNode).AsSectionField<TextLabel>(),
                             Map(x => x.SourceNode).AsSectionField<TextLabel>(),
                             Map(x => x.TransactionDate).AsSectionField<TextLabel>(),
                             Map(x=>x.TransactionType).AsSectionField<TextLabel>(),
                         }
                        ),
                    });
       }
    }
}
