using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankSwitch.UI.SchemeManagement
{
   public class TransTypeChannelsFeeDetails:EntityUI<TransactionTypeChannelFee>
    {
       public TransTypeChannelsFeeDetails()
       {
           UseFullView();
           WithTitle("Transaction_Type-Channels-Fee Details");
           AddSection()
               .WithTitle("Channels Details")
               .IsFormGroup()
               .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Channel.Name).AsSectionField<TextLabel>(),
                             Map(x => x.Channel.Code).AsSectionField<TextLabel>().LabelTextIs("Channels Code"),
                             Map(x =>x.Channel.Description).AsSectionField<TextLabel>(),
                        }),
                    });
           AddSection().WithTitle("Transaction Type Details")
              .IsFormGroup()
              .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.TransactionType.Name).AsSectionField<TextLabel>(),
                             Map(x => x.TransactionType.Code).AsSectionField<TextLabel>().LabelTextIs("Transaction Code"),
                             Map(x =>x.TransactionType.Description).AsSectionField<TextLabel>().LabelTextIs("Description"),
                        }),
                    });
           AddSection().WithTitle("Transaction Fee Details")
             .IsFormGroup()
             .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Fee.Name).AsSectionField<TextLabel>().LabelTextIs("Name"),
                             Map(x => x.Fee.FlatAmount).AsSectionField<TextLabel>().LabelTextIs("Flat Amount"),
                             Map(x => x.Fee.PercentageOfTransaction + "%").AsSectionField<TextLabel>().LabelTextIs("Percentage of Transaction"),
                             Map(x =>x.Fee.Maximum).AsSectionField<TextLabel>().LabelTextIs("Maximum"),
                             Map(x =>x.Fee.Minimum).AsSectionField<TextLabel>().LabelTextIs("Minimum"),
                       }),
          });
       }
    }
}
