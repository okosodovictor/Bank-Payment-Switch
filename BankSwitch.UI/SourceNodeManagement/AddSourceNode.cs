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
   public class AddSourceNode:EntityUI<SourceNode>
    {
       public AddSourceNode()
       {
           string err = "";
           AddSection()
               .WithTitle("Add New Source Node")
               .WithFields(
               new List<IField>()
                {
                    Map(x => x.Name).AsSectionField<TextBox>().TextFormatIs(TextFormat.name).WithLength(50).Required(),
                    Map(x => x.HostName).AsSectionField<TextBox>().TextFormatIs(@"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9-]*[a-zA-Z0-9]).)*([A-Za-z]|[A-Za-z][A-Za-z0-9-]*[A-Za-z0-9])$").WithLength(50).Required(),
                    Map(x => x.IPAddress).AsSectionField<TextBox>().WithLength(20).TextFormatIs(@"^(0[0-7]{10,11}|0(x|X)[0-9a-fA-F]{8}|(\b4\d{8}[0-5]\b|\b[1-3]?\d{8}\d?\b)|((2[0-5][0-5]|1\d{2}|[1-9]\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))(\.((2[0-5][0-5]|1\d{2}|\d\d?)|(0(x|X)[0-9a-fA-F]{2})|(0[0-7]{3}))){3})$").Required(),
                    Map(x => x.Port).AsSectionField<TextBox>().TextFormatIs(TextFormat.numeric).WithLength(10).Required(),
                     Map(x => x.Schemes).AsSectionField<MultiSelect>().Of<Scheme>(()=>new SchemeManager().RetrieveAll())
                             .WithColumn(x => x.Name)
                             .WithColumn(x=>x.Route.Name,"Route")
                             .WithColumn(x=>x.Description)
                             .WithColumn(x=>x.TransactionTypeChannelFees.Count,"TransactionTypeChannelFee Count")
                             .ListOf(x=>x.Name, x=>x.Id)
                             .WithTypeAhead().Required().LabelTextIs("Schemes"),
                });
           AddButton()
           .WithText("Add Source Node")
           .ConfirmWith("Please confirm operation!")
           .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.Disk))
           .SubmitTo(x =>
           {
               bool result = false;
               try
               {
                   x.Schemes = x.Schemes;
                var scheme= new SourceNodeManager().CreateSourceNode(x);
                if (scheme != null)
                {
                    result = true;
                }
                return result;
               }
               catch (Exception ex)
               {
                   err = ex.Message;
                   return false;
               }
           }).OnSuccessDisplay("New Source Node successfully Created").OnFailureDisplay(string.Format("Failed to add new Source Node. Possible Reason:{0}", err));
       }
    }
}
