using AppZoneUI.Framework;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using BankSwitch.UI.Model;
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
           AddSection().IsFramed().IsCollapsible().WithTitle("TransactionType-Channels-Fee")
               .WithColumns(new List<Column>()
                    {
                        
                      new Column(  
                     new List<IField> { 
                            Map(x => x.TypeUI)
                                    .AsSectionField<DropDownList>()
                                    .Of(new TransactionTypeManager().GetAllTransactionType())
                                    .ListOf(x => x.Name, x => x.Id).LabelTextIs("TransactionType"),
                              Map(x => x.ChannelUI)
                                    .AsSectionField<DropDownList>()
                                    .Of(new ChannelManager().GetAllChannel())
                                    .ListOf(x => x.Name, x => x.Id),
                              Map(x => x.FeeUI)
                                    .AsSectionField<DropDownList>()
                                    .Of(new FeeManager().GetFees())
                                    .ListOf(x => x.Name, x => x.Id),

                                    AddSectionButton().WithText("Add Transaction_Channel_Fee")
                                        .UpdateWith(x => 
                                          {
                                               TransactionTypeChannelFee trnx = new TransactionTypeChannelFee();

                                                  if (x.ChannelUI != null) trnx.Channel = x.ChannelUI;
                                                  if (x.FeeUI != null) trnx.Fee = x.FeeUI;
                                                  if (x.TypeUI != null) trnx.TransactionType = x.TypeUI;
                                                  if (x.TransactionTypeChannelFees.Any(s => s.Channel == x.ChannelUI && s.Fee == x.FeeUI))
                                                  {
                                                      x.TransactionTypeChannelFees.Remove(trnx);
                                                  }
                                                  else
                                                  { 
                                                  x.TransactionTypeChannelFees.Add(trnx);
                                                  }
                                              return x;
                                          }),
                         })
                    });

           HasMany(x => x.TransactionTypeChannelFees).As<Grid>()
                    .Of<TransactionTypeChannelFee>()
                    .WithColumn(x => x.Channel.Name, "Channel Name")
                    .WithColumn(x => x.Fee.Name, "Fee")
                    .WithColumn(x => x.TransactionType.Name, "Transaction Type")
                    .IsPaged(10);
                           
           AddButton()
               .ConfirmWith("Please confirm operation!")
               .WithText("UPDATE")
               .SubmitTo(x =>
               {
                   var result = false;
                   try
                   {
                     object  schemeobject = new SchemeManager().UpdateScheme(x);
                       if(schemeobject!=null)
                       {
                           result = true;
                       }
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
