using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.SourceNodeManagement
{
   public class SourceNodeSchemeComboDetails:EntityUI<TransactionTypeChannelFee>
    {
       public SourceNodeSchemeComboDetails()
       {
           UseFullView();
           WithTitle("Transaction");
           AddSection().WithTitle("Channel Details")
               .IsFormGroup()
               .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                            Map(x => x.Channel.Name).AsSectionField<TextLabel>().LabelTextIs("Name"),
                            Map(x => x.Channel.Code).AsSectionField<TextLabel>().LabelTextIs("Code"),
                            Map(x =>x.Channel.Description).AsSectionField<TextLabel>().LabelTextIs("Description"),
                        }),
                    });
           AddSection().WithTitle("Transaction Type Details")
              .IsFormGroup()
              .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.TransactionType.Name).AsSectionField<TextLabel>(),
                             Map(x => x.TransactionType.Code).AsSectionField<TextLabel>(),
                             Map(x =>x.TransactionType.Description).AsSectionField<TextLabel>(),
                        }),
                    });
           AddSection().WithTitle("Fee Details")
             .IsFormGroup()
             .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Fee.Name).AsSectionField<TextLabel>(),
                             Map(x => PercentageOfTransaction(x.Fee.PercentageOfTransaction.ToString()))
                                    .AsSectionField<TextLabel>().LabelTextIs("Percentage Of Transaction"),
                             Map(x =>x.Fee.Maximum).AsSectionField<TextLabel>().LabelTextIs("Maximum"),
                             Map(x =>x.Fee.Minimum).AsSectionField<TextLabel>().LabelTextIs("Minimum"),
                        }),
                    });
       }
       public static string PercentageOfTransaction(String Param)
       {
           return Param + "%";
       }
    }
}
