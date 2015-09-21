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

namespace BankSwitch.UI.SinkNodeManagement
{
    public class ViewSinkNodeList : EntityUI<SinkNodeModel>
    {
       public ViewSinkNodeList()
       {
            WithTitle("View Sink Nodes");
             AddSection().WithTitle("Search")
             .IsFramed()
             .IsCollapsible()
            .WithFields(new List<IField>()
             {

                       Map(x => x.Name).AsSectionField<TextBox>().WithLength(10),
                       Map(x => x.HostName).AsSectionField<TextBox>().LabelTextIs("Host Name").WithLength(10),
                        AddSectionButton()
                       .WithText("Search")
                       .UpdateWith(x =>
                                    {
                                        return x;
                                    })
                                });

               
                 Map(x =>x.SinkNodes).As<Grid>()
                .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.Link))
                .ApplyMod<ViewDetailsMod>(y => y.Popup<SinkNodeDetail>("Sink Node Details")
                .PrePopulate<SinkNode, SinkNode>(x => x))
               .Of<SinkNode>()
               .WithColumn(x => x.Name, "Node Name")
               .WithColumn(x => x.HostName, "Host Name")
               .WithColumn(x => x.IPAddress, "IP Address")
               .WithColumn(x => x.Port)
               .WithColumn(x => x.IsActive?"Active":"Inactive","Status")
               .IsPaged<SinkNodeModel>(10, (x, e) =>
               {
                   int totalCount = 0;
                   try
                   {
                       x.SinkNodes = new SinkNodeManager().GetSinkNodes(x.Name, x.HostName, x.IPAddress);
                       e.TotalCount = totalCount * e.Limit;
                   }
                   catch (Exception)
                   {

                       throw;
                   }
                   return x;
               });
        }
       }
}
