using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.TransactionTypeManagement
{
   public class TrnsactionTypeDetail:EntityUI<TransactionType>
    {
       public TrnsactionTypeDetail()
        {
            UseFullView();
            WithTitle("View Transaction Type Details");
            AddSection()
                .IsFormGroup()
                .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Name).AsSectionField<TextLabel>(),
                             Map(x => x.Code).AsSectionField<TextLabel>(),
                             Map(x => x.Description).AsSectionField<TextLabel>(),
                        }
                        ),
                    });
                 AddButton().WithText("Edit Transaction Type")
                 .ApplyMod<ButtonPopupMod>(x => x
                 .Popup<EditTransactionType>("Edit Transaction"));
        }
    }
}
