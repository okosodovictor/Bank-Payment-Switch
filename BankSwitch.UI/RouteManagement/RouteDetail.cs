using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.RouteManagement
{
    public class RouteDetail:EntityUI<Route>
    {
        public RouteDetail()
        {
             UseFullView();
            WithTitle("Route Details");
            AddSection().IsFormGroup()
                .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Name).AsSectionField<TextLabel>().LabelTextIs("Name"),
                             Map(x => x.CardPAN).AsSectionField<TextLabel>().LabelTextIs("CardPAN"),
                             Map(x => x.Description).AsSectionField<TextLabel>().LabelTextIs("Description"),
                        }
                        ),
                    });
            AddSection().WithTitle("Sink Details").IsFormGroup()
           .WithColumns(new List<Column>()
             {

                    new Column(new List<IField>()
                {
                          Map(x => x.SinkNode.Name).AsSectionField<TextLabel>().LabelTextIs("Name"),
                          Map(x => x.SinkNode.HostName).AsSectionField<TextLabel>().LabelTextIs("Host"),
                          Map(x => x.SinkNode.Port).AsSectionField<TextLabel>().LabelTextIs("Port"),
                          Map(x => x.SinkNode.IPAddress).AsSectionField<TextLabel>().LabelTextIs("IP Address"),
                          Map(x => x.SinkNode.IsActive).AsSectionField<TextLabel>().LabelTextIs("State"),
               })
             }
             );
            AddButton().WithText("Edit Route")
                .ApplyMod<ButtonPopupMod>(x => x.Popup<EditRoute>("Edit Route"));
               //.PrePopulate<Route, Route>(y => y));
        }
    }
}
