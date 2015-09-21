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

namespace BankSwitch.UI.SourceNodeManagement
{
   public class ViewSourceNodeList:EntityUI<SourceNodeModel>
    {
       public ViewSourceNodeList()
       {
           WithTitle("View Source Nodes");
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
                                        return x;
                                    })
                  
             });
           AddSection().WithTitle("Source Nodes").IsFramed().IsCollapsible()
           .WithColumns(new List<Column>()
                {
                    new Column(new List<IField>()
                    {
                            HasMany(x => x.SourceNodes)
                            .AsSectionField<Grid>().LabelTextIs("Source Node")
                            .Of<SourceNode>()
                            .WithRowNumbers()
                            .WithColumn(x => x.Name)
                            .WithColumn(x => x.HostName)
                            .WithColumn(x => x.IPAddress)
                            .WithColumn(x => x.Port)
                            .WithColumn(x => x.IsActive?"Active":"InActive","Status")
                            .WithRowNumbers()
                            .IsPaged<SourceNodeModel>(10, (x, e) =>
                            {
                                int totalCount = 0;
                                try
                                {
                                    x.SourceNodes = new SourceNodeManager().Search(x.Name, e.Start, e.Limit, out totalCount);
                                    e.TotalCount = totalCount;
                                    return x;
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                e.TotalCount = totalCount * e.Limit;
                                 return x;
                            }).ApplyMod<ViewDetailsMod>(y => y.Popup<sourceNodeDetail>("Source Node Details")),
                
                   })
                });
       }
    }
}
