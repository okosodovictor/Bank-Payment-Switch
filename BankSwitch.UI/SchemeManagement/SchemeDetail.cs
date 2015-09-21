using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.SchemeManagement
{
   public class SchemeDetail:EntityUI<Scheme>
    {
       public SchemeDetail()
       {
           UseFullView();
           WithTitle("View Scheme Details");
           AddSection()
               .IsFormGroup()
               .WithColumns(new List<Column>()
                    {
                        new Column( new List<IField>()
                        {
                             Map(x => x.Name).AsSectionField<TextLabel>(),
                             Map(x => x.Route.Name).AsSectionField<TextLabel>(),
                             Map(x => x.Description).AsSectionField<TextLabel>(),

                             AddSectionButton().WithText("Edit Scheme")
                                .ApplyMod<ButtonPopupMod>(x => x.Popup<EditScheme>("Edit")
                                    .PrePopulate<Scheme, Scheme>(y => y)),
                        }),
                    });
           AddSection().IsFramed().IsCollapsible()
           .WithColumns(new List<Column>()
            {
                new Column(new List<IField>()
                {
                         HasMany(x => x.TransactionTypeChannelFees)
                        .AsSectionField<Grid>() 
                        .Of<TransactionTypeChannelFee>()
                        .WithColumn(x => x.TransactionType.Name, "Transation Type")
                        .WithColumn(x => x.Channel.Name, "Channel")
                        .WithColumn(x => x.Fee.Name,"Fee")
                        .WithRowNumbers()
                        .IsPaged<Scheme>(10, (x, e) =>
                        {
                            //x.SchemeList = EntityDb<Scheme>.GetAll();
                            return x;
                         })
                         .ApplyMod<ViewDetailsMod>(y => y.Popup<TransTypeChannelsFeeDetails>("View Details"))
                })
            });
       }
    }
}
