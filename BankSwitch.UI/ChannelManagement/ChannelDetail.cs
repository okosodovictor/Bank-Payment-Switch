using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.ChannelManagement
{
   public class ChannelDetail:EntityUI<Channel>
    {
       public ChannelDetail()
       {
           UseFullView();
           WithTitle("View Channel Details");
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
           AddButton().WithText("Edit Channel")
               .ApplyMod<ButtonPopupMod>(x => x
               .Popup<EditChannel>("Edit Channel"));
           //.PrePopulate<Channels, Channels>(y => y));
       }
    }
}
