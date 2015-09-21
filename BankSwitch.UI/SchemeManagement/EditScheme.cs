using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.SchemeManagement
{
   public class EditScheme:EntityUI<Scheme>
    {
       public EditScheme()
       {
           string err = "";
           AddSection()
                .WithTitle("Edit Scheme")
                .WithFields(
                new List<IField>()
                {
                    Map(x => x.Name).AsSectionField<TextBox>().Required().TextFormatIs(TextFormat.name).WithLength(100),
                    Map(x => x.Route).AsSectionField<DropDownList>().Of(new RouteManager().GetAllRoute()).ListOf(x=>x.Name, x=>x.Id).LabelTextIs("Route"),
                    Map(x => x.Description).AsSectionField<TextArea>().Required().TextFormatIs(TextFormat.name).WithLength(300),
                });
           AddButton()
               .ConfirmWith("Please confirm operation!")
               .WithText("UPDATE")
               .SubmitTo(x =>
               {
                   var result = false;
                   try
                   {
                      // result = 
                   }
                   catch (Exception ex)
                   {
                       err = ex.Message;
                       throw;
                   }
                   return result;
               }).OnSuccessDisplay("Scheme Successfull Updated").OnFailureDisplay(string.Format("Failed to Update:{0}", err));
       }
    }
}
