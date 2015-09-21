using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankSwitch.UI.RouteManagement
{
    public class EditRoute : EntityUI<Route>
    {
        public EditRoute()
        {
            WithTitle("Edit Route");
            AddSection()
           .WithFields(
           new List<IField>()
                {
                    Map(x => x.Name).AsSectionField<TextBox>().TextFormatIs(TextFormat.name).WithLength(100).Required(),
                    Map(x => x.CardPAN).AsSectionField<TextBox>().TextFormatIs(TextFormat.numeric).LabelTextIs("Card PAN").WithLength(6).Required(),
                    Map(x => x.Description).AsSectionField<TextArea>().TextFormatIs(TextFormat.name).WithLength(300).Required(),
                    Map(x => x.SinkNode).AsSectionField<DropDownList>().Of(new SinkNodeManager().GetAllSinkNode()).ListOf(x=>x.Name, x=>x.Id).LabelTextIs("Sink Node"),
                
                });
            AddSectionButton()
              .WithText("UPDATE")
              .ConfirmWith("Please confirm operation!")
              .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.Disk))
              .SubmitTo(x =>
              {
                  var result = false;
                  try
                  {
                      result = new RouteManager().EditRoute(x);
                  }
                  catch (Exception)
                  {
                      throw;
                  }
                  return result;
              }).OnSuccessDisplay("Route successfully Updated").OnFailureDisplay("Failed to Update. Possible Reason: Duplicate Card PAN.");
        }
    }
}

