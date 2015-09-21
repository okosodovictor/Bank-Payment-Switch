using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.SinkNodeManagement
{
   public class SinkNodeDetail:EntityUI<SinkNode>
    {
       public SinkNodeDetail()
       {
           UseFullView();
           WithTitle("View Sink Node Details");
           AddSection()
               .IsFormGroup()
               .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Name).AsSectionField<TextLabel>(),
                             Map(x => x.HostName).AsSectionField<TextLabel>(),
                             Map(x => x.IPAddress).AsSectionField<TextLabel>(),
                             Map(x => x.Port).AsSectionField<TextLabel>(),
                             Map(x => x.IsActive).AsSectionField<TextLabel>(),
                         }
                        ),
                    });
           AddButton().WithText("Edit Sink Node")
               .ApplyMod<ButtonPopupMod>(x => x
               .Popup<EditSinkNode>("Edit Sink Node"));

           AddButton().WithText(x=>x.IsActive?"Disable Node":"Enable Node")
               .SubmitTo(x=>
                  {
                     if(x.IsActive)
                     {
                         x.IsActive = false;
                     }
                     else
                     {
                         x.IsActive = true;
                     }
                     var result = new SinkNodeManager().Edit(x);
                      return result;
                  });
       }
    }
}
