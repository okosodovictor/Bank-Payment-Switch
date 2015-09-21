using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankSwitch.UI.FeeManagement
{
   public class EditFee:EntityUI<Fee>
    {
       string message = "";
       public EditFee()
       {
             AddSection()
             .IsFramed()
             .WithTitle("Edit Fee")
             .WithColumns(new List<Column> 
            { 
                new Column(  
                     new List<IField> { 
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
                    }),  
            })
             .WithFields(new List<IField>{
                   AddSectionButton()
                       .SubmitTo(f=> 
                       {
                           try
                           {
                               bool result = new FeeManager().EditFee(f);
                               return true;
                           }
                           catch (Exception ex)
                           {
                               message = ex.Message; ;
                               throw;
                           }
                      }) 
                    .ConfirmWith (s => String.Format("Update Fee {0} ", s.Name)).WithText("Update")
                    .OnSuccessDisplay(s => String.Format("Update Fee {0} has been updated ", s.Name))
                    .OnFailureDisplay(s => String.Format("Error: Fee{0} was not updated ", message))
                    
              });  
        }
     }
}
