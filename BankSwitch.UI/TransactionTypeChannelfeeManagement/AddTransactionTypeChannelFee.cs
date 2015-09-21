using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.TransactionTypeChannelfeeManagement
{
   public class AddTransactionTypeChannelFee:EntityUI<TransactionTypeChannelFee>
    {
       public AddTransactionTypeChannelFee()
       {
           string err = "";
           WithTitle("Add TransactionCombo");
           AddSection()
               .WithTitle("TransactionCombo Information")
               //.StretchFields(30).IsFramed()
               .IsCollapsible()
               .WithFields(new List<IField>()
                            {
                                Map(x => x.TransactionType)
                                        .AsSectionField<DropDownList>()
                                        .Of(new TransactionTypeManager().GetAllTransactionType())
                                        .ListOf(x => x.Name, x => x.Id)
                                        .Required(),
                                Map(x => x.Channel)
                                        .AsSectionField<DropDownList>()
                                        .Of(new ChannelManager().GetAllChannel())
                                        .ListOf(x => x.Name, x => x.Id),
                                Map(x => x.Fee)
                                        .AsSectionField<DropDownList>()
                                        .Of(new FeeManager().GetFees())
                                        .ListOf(x => x.Name, x => x.Id)
                            });

           AddButton().WithText("Save")
               .SubmitTo(ch =>
               {
                   return new TransactionTypeChannelFeeManager().CreateTransactionTypeChannelFee(ch);
               })
               .OnSuccessDisplay("Successfully Saved")
               .OnFailureDisplay("Sorry!!! Transaction Combo Not Saved")
               .CssClassIs("btn btn-default");
       }
    }
}
