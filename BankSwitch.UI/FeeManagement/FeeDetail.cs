using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.FeeManagement
{
   public class FeeDetail:EntityUI<Fee>
    {
       public FeeDetail()
       {
           UseFullView();
           WithTitle("View Fee Details");
           AddSection()
               .IsFormGroup()
               .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Name).AsSectionField<TextLabel>(),
                             Map(x => x.FlatAmount).AsSectionField<TextLabel>().LabelTextIs("Flat Amount"),
                             Map(x => x.PercentageOfTransaction).AsSectionField<TextLabel>().LabelTextIs("Percent of Transaction"),
                             Map(x => x.Maximum).AsSectionField<TextLabel>(),
                             Map(x => x.Minimum).AsSectionField<TextLabel>(),
                        }
                        ),
                    });
               AddButton().WithText("Edit Fee")
               .ApplyMod<ButtonPopupMod>(x => x
               .Popup<EditFee>("Edit Fee"));
       }
    }
}
