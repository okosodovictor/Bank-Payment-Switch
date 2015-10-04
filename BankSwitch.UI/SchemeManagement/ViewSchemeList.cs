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

namespace BankSwitch.UI.SchemeManagement
{
   public class ViewSchemeList:EntityUI<Scheme>
    {
       public ViewSchemeList()
       {
           WithTitle("View Schemes");
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
           AddSection().WithTitle("Scheme").IsFramed().IsCollapsible()
           .WithColumns(new List<Column>()
                {
                    new Column(new List<IField>()
                    {
                            HasMany(x => x.Schemes)
                            .AsSectionField<Grid>().LabelTextIs("Scheme")
                            .Of<Scheme>()
                            .WithRowNumbers()
                            .WithColumn(x => x.Name)
                            .WithColumn(x => x.Route.Name,"Route")
                            .WithColumn(x => x.Description)
                           // .WithColumn(x => x.TransactionTypeChannelFees.Count, "TransactionTypeChannelFeeList Count")
                            .WithRowNumbers()
                            .IsPaged<Scheme>(10, (x, e) =>
                            {
                                int totalCount = 0;
                                try
                                {
                                    x.Schemes = new SchemeManager().Search(x.Name, e.Start, e.Limit, out totalCount);
                                    e.TotalCount = totalCount;
                                    return x;
                                }
                                catch (Exception)
                                {
                                    throw ;
                                }
                            }).ApplyMod<ViewDetailsMod>(y => y.Popup<SchemeDetail>("Scheme Details")),
                
                   })
                });
       }
    }
}
