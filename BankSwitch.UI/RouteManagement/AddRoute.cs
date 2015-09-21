using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.RouteManagement
{
   public class AddRoute:EntityUI<Route>
    {
       public AddRoute()
       {
           AddSection()
            .WithTitle("Transaction Type Management")
                .WithTitle("Add New ")
                .StretchFields(30)
                .IsCollapsible()
                .IsFramed()
            .WithFields(new List<IField>()
                {
                    Map(x => x.Name).AsSectionField<TextBox>().TextFormatIs(TextFormat.name).WithLength(100).Required(),
                    Map(x => x.CardPAN).AsSectionField<TextBox>().TextFormatIs(TextFormat.numeric).LabelTextIs("Card PAN").WithLength(6).Required(),
                    Map(x => x.Description).AsSectionField<TextArea>().TextFormatIs(TextFormat.name).WithLength(300).Required(),
                    Map(x => x.SinkNode).AsSectionField<DropDownList>().Required().Of(new SinkNodeManager().GetAllSinkNode()).ListOf(x=>x.Name,x=>x.Id),
                    AddSectionButton()
                    .ConfirmWith("Please confirm operation!")
                    .WithText("Save")
                    .SubmitTo(x =>
                        {
                            var result = false;
    
                            try
                            {

                                result = new RouteManager().CreateRoute(x);
                           
                            }
                            catch(Exception)
                            {
                                throw;
                            }
                            return result;
                        }).OnSuccessDisplay("Successfully Saved").OnFailureDisplay("Failed to Update. Possible Reason: 1. Duplicate Name.\n 2. Duplicate Card PAN"),
             });
       }
    }
}
