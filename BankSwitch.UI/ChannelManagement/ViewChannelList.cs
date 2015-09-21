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

namespace BankSwitch.UI.ChannelManagement
{
   public class ViewChannelList:EntityUI<ChannelModel>
    {
       public ViewChannelList()
       {
           WithTitle("View Channels");
           AddSection().WithTitle("Search")
           .IsFramed()
           .IsCollapsible()
          .WithFields(new List<IField>()
             {
                        Map(x => x.Name).AsSectionField<TextBox>().WithLength(10),
                        Map(x => x.Code).AsSectionField<TextBox>().WithLength(10),
                        AddSectionButton()
                       .WithText("Search")
                       .UpdateWith(x =>
                                    {
                                        return x;
                                    })
                  
                                    });
             AddSection().WithTitle("Channels").IsFramed().IsCollapsible()
             .WithColumns(new List<Column>()
                {
                    new Column(new List<IField>()
                    {
                            HasMany(x => x.Channels)
                            .AsSectionField<Grid>().LabelTextIs("Channel")
                            .Of<Channel>()
                            .WithRowNumbers()
                            .WithColumn(x => x.Name)
                            .WithColumn(x => x.Code)
                            .WithColumn(x => x.Description)
                            .WithRowNumbers()
                            .IsPaged<ChannelModel>(10, (x, e) =>
                            {
                                int totalCount = 0;
                                try
                                {
                                    x.Channels = new ChannelManager().RetreiveChannels(x.Name, x.Code);
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                                e.TotalCount = totalCount * e.Limit;
                                 return x;
                            }).ApplyMod<ViewDetailsMod>(y => y.Popup<ChannelDetail>("Channel Details")),
                
                   })
           });
       }
    }
}
