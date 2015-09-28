using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.TransactionTypeManagement
{
   public class AddTransactionType:EntityUI<TransactionType>
    {
       public AddTransactionType()
       {

           string error = "";
           WithTitle("Add Transaction Type");
           AddSection()
               .WithTitle("Transaction Type Information")
               .IsCollapsible()
               .WithFields(new List<IField>()
                            {
                                Map(x => x.Name)
                                        .AsSectionField<TextBox>()
                                        .Required()
                                        .WithLength(15),
                                Map(x => x.Code)
                                        .AsSectionField<TextBox>()
                                        .Required()
                                        .TextFormatIs(TextFormat.numeric)
                                        .WithLength(15),
                                Map(x => x.Description)
                                        .AsSectionField<TextBox>()
                                        .Required()
                            });

           AddButton().WithText("Save")
               .SubmitTo(trnx =>
               {
                   try
                   {
                       var result = new TransactionTypeManager().AddTransactionType(trnx);
                   }
                   catch (Exception ex)
                   {
                       while (ex.InnerException != null) ex = ex.InnerException;
                       error = ex.Message;
                       throw;
                   }
                   return true;
               })
               .OnSuccessDisplay("Successfully Saved")
               .OnFailureDisplay(string.Format("Sorry!!! Transaction Type Not Saved{0}", error))
               .CssClassIs("btn btn-default");
       }
    }
}
