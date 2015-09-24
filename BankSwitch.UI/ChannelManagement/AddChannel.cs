using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.ChannelManagement
{
   public class AddChannel:EntityUI<Channel>
    {
       public AddChannel( )
       {
           string  error = "";
           WithTitle("Add Channel");
           AddSection()
               .WithTitle("Channel Information")
               //.StretchFields(30).IsFramed()
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
               .SubmitTo(ch =>
               {
                var result = false;
                try 
	            {
                    result = new ChannelManager().CreateChannel(ch);
	            }
                catch (Exception ex)
                {
		             error = ex.Message;
	                throw;
                }
                return result;
               })
               .OnSuccessDisplay("Successfully Saved")
               .OnFailureDisplay(string.Format("Sorry!!! Channel Not Saved:{0}", error))
               .CssClassIs("btn btn-default");
       }
    }
}
