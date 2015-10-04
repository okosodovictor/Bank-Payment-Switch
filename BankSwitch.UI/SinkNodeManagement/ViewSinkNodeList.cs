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
           AddSection()
           .IsFramed()
           .IsCollapsible()
              .WithColumns(
                  new List<Column>()
                    {
                        new Column(new List<IField>()
                        {  
                           Map(x => x.Name).AsSectionField<TextBox>().LabelTextIs("Name"),
                           Map(x => x.Port).AsSectionField<TextBox>().WithLength(30),
                              AddSectionButton()
                            .WithText("Search")
                            .UpdateWith(x=> 
                                {
                                    return x;
                                })
                            }),
                              new Column(
                            new List<IField>()
                            {
                              Map(x => x.HostName).AsSectionField<TextBox>().LabelTextIs("Host Name"),
                               Map(x=>x.IPAddress).AsSectionField<TextBox>().WithLength(30),
                            }),
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
                   int total = 0;
                   try
                   {
                       x.SinkNodes = new SinkNodeManager().GetSinkNodes(x.Name, x.HostName, x.IPAddress, x.Port, e.Start / e.Limit, e.Limit, out total);
                       e.TotalCount = total;
                       System.Web.HttpContext.Current.Session["TotalSinkNode"] = e.TotalCount;
                       return x;
                   }
                   catch (Exception)
                   {

                       throw;
                   }
               });
        }
       }
}
