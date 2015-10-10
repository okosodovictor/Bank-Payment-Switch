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
            string err = "";
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
            AddButton().WithText("Update")
                .ConfirmWith("Please confirm operation!")
                .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.Disk))
                .SubmitTo(x =>
                {

                    bool result = false;
                    try
                    {
                        result = new RouteManager().EditRoute(x);
                    }
                    catch (Exception ex)
                    {
                        err = ex.Message;
                    }
                    return result;
                }).OnSuccessDisplay(" Route successfully Updated").OnFailureDisplay(string.Format("Failed to Update Route:{0}", err));
        }
    }
}

