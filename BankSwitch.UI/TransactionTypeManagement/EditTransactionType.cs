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
   public class EditTransactionType:EntityUI<TransactionType>
    {
       public EditTransactionType()
       {

           AddSection()
                .IsFramed()
                .WithTitle("Edit TransactionType")
                .WithColumns(new List<Column> 
            { 
                new Column(  
                     new List<IField> { 
                          Map(x=>x.Name)
                                .AsSectionField<TextBox>() 
                                .WithLength(50).LabelTextIs("Name") 
                                .Required().TextFormatIs(""), 
                          Map(x=>x.Code)
                                .AsSectionField<TextBox>()
                                .TextFormatIs(TextFormat.numeric)
                                .WithLength(60).LabelTextIs("Code").Required(), 
                          Map(x=>x.Description)
                                .AsSectionField<TextBox>()
                                .WithLength(60).LabelTextIs("Description").Required(),
                    }),  
            })
                .WithFields(new List<IField>{
                   AddSectionButton()
                       .SubmitTo(trnx => 
                       {
                           return new TransactionTypeManager().Edit(trnx);
                       }) 
                    .ConfirmWith (s => String.Format("Update TransactionType {0} ", s.Name)).WithText("Update")
                    .OnSuccessDisplay(s => String.Format("Update TransactionType {0} has been updated ", s.Name))
                    .OnFailureDisplay(s => String.Format("Error: TransactionType{0} was not updated ", s.Name))
                    
              });
       }
    }
}
