using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.FeeManagement
{
   public class AddFee:EntityUI<Fee>
    {
       public AddFee()
       {
           string message = "";
           WithTitle("Add Fee");
           AddSection()
               .WithTitle("Fee Information")
               //.StretchFields(30).IsFramed()
               .IsCollapsible()
               .WithFields(new List<IField>()
                            {
                                Map(x => x.Name)
                                        .AsSectionField<TextBox>()
                                        .Required()
                                        .WithLength(15),
                                Map(x => x.FlatAmount)
                                        .AsSectionField<TextBox>()
                                        .Required()
                                        .TextFormatIs(TextFormat.numeric)
                                        .WithLength(15),
                                Map(x => x.PercentageOfTransaction)
                                        .AsSectionField<TextBox>()
                                        .Required()
                                        .TextFormatIs(TextFormat.numeric),
                                Map(x => x.Maximum)
                                        .AsSectionField<TextBox>()
                                        .Required()
                                        .TextFormatIs(TextFormat.numeric)
                                        .WithLength(15),
                                Map(x => x.Minimum)
                                        .AsSectionField<TextBox>()
                                        .Required()
                                        .TextFormatIs(TextFormat.numeric)
                            });

           AddButton().WithText("Save")
               .SubmitTo(fee =>
               {
                   
                  try 
	             {	        
	        	 bool result = new FeeManager().CreateFee(fee);
                 return result;
	            }
	           catch (Exception ex)
	          {
		        message = ex.Message;
		         throw;
	          }
               })
               .OnSuccessDisplay("Successfully Saved")
               .OnFailureDisplay(string.Format("Sorry!!! Fee Not Saved",message))
               .CssClassIs("btn btn-default");
       }
    }
}
