using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using BankSwitch.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.RouteManagement
{
   public class ViewRouteList:EntityUI<RouteModel>
    {
       public ViewRouteList()
       {
           WithTitle("View Routes");
           AddSection().WithTitle("Search")
           .IsFramed()
           .IsCollapsible()
          .WithFields(new List<IField>()
             {
                        Map(x => x.Name).AsSectionField<TextBox>().WithLength(10),
                        AddSectionButton()
                       .WithText("Search")
                       .UpdateWith(x =>
                                    {
                                        //EntityDb<Route>.GetByLinqQuery(z => z.Name == x.Name).ToList();
                                        return x;
                                    })
             });
           AddSection().WithTitle("Routes").IsFramed().IsCollapsible()
           .WithColumns(new List<Column>()
                {
                    new Column(new List<IField>()
                    {
                            HasMany(x =>x.Routes)
                            .AsSectionField<Grid>()
                            .Of<Route>()
                            .WithRowNumbers()
                            .WithColumn(x => x.Name)
                            .WithColumn(x => x.CardPAN)
                            .WithColumn(x => x.Description)
                            .WithColumn(x => x.SinkNode.Name, "Sink Node")
                            .WithRowNumbers()
                            .IsPaged<RouteModel>(10, (x, e) =>
                            {
                                x.Routes = new RouteManager().RetreiveRouteByName(x.Name);
                                return x;
                            }).ApplyMod<ViewDetailsMod>(y => y.Popup<RouteDetail>("Route Details")),
                
                   })
                });
       }
    }
}
