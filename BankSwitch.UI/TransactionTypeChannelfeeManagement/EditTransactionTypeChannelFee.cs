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
   public class EditTransactionTypeChannelFee:EntityUI<TransactionTypeChannelFee>
    {
       public EditTransactionTypeChannelFee()
       {
           AddSection()
          .IsFramed()
          .WithTitle("Edit TransactionCombo")
          .WithColumns(new List<Column> 
            { 
                new Column(  
                     new List<IField> { 
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
                    }),  
            })
          .WithFields(new List<IField>{
                   AddSectionButton()
                       .SubmitTo(                       
                      ch => 
                      {
                          return new TransactionTypeChannelFeeManager().Edit(ch);
                      }) 
                    .OnSuccessDisplay("Successful")
                    .OnFailureDisplay("Failed")
                    
              });
       }
    }
}
