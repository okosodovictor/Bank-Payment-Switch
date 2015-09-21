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
   public class sourceNodeDetail:EntityUI<SourceNode>
    {
       public sourceNodeDetail()
       {
           UseFullView();
           WithTitle("View Source Node Details");
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
                             //Map(x => x.SchemeList).AsSectionField<TextLabel>(),
                             Map(x => x.IsActive).AsSectionField<TextLabel>(),

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
                     var result = new SourceNodeManager().EditSourceNode(x);
                      return result;
                  }),

                AddSectionButton().WithText("Edit Source Node")
                .ApplyMod<ButtonPopupMod>(x => x
                .Popup<EditSourceNode>("Edit Source Node")),
                 }),
               });


          // AddSection().IsFramed().IsCollapsible().WithTitle("Scheme List")
          //.WithColumns(new List<Column>()
          //  {
          //      new Column(new List<IField>()
          //      {
          //               HasMany(x => x.Schemes)
          //              .AsSectionField<Grid>() 
          //              .Of<Scheme>()
          //              .WithColumn(x => x.Name)
          //              .WithColumn(x => x.Route.Name,"Route")
          //              .WithColumn(x => x.Description,"Description")
          //              .WithColumn(x => x.TransactionTypeChannelFees.Count,"TransactionTypeChannelFee Count")
          //              .WithRowNumbers()
          //              .IsPaged<SourceNodeModel>(10, (x, e) =>
          //              {
          //                  x.SourceNodes = new SourceNodeManager().RetrieveAll();
          //                  return x;
          //               })
          //              .ApplyMod<ViewDetailsMod>(y => y.Popup<SourceNodeComboDetail>("View Details"))
          //      })
          //  });
       }
    }
}
