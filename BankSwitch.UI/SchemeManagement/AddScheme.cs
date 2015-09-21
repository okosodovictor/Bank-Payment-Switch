using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using BankSwitch.UI.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BankSwitch.UI.SchemeManagement
{
    public class AddScheme : EntityUI<SchemeModel>
    {
        public AddScheme()
        {
            List<TransactionTypeChannelFee> list = new List<TransactionTypeChannelFee>();
            WithTitle("Scheme Management");
            AddSection().WithTitle("Add New Scheme")
                .IsCollapsible()
                .IsFramed()
            .WithFields(new List<IField>()
            {
                    Map(x => x.Name).AsSectionField<TextBox>().TextFormatIs(TextFormat.name).WithLength(100),
                    Map(x => x.Route).AsSectionField<DropDownList>().Of(new RouteManager().GetAllRoute()).ListOf(x=>x.Name, x=>x.Id).LabelTextIs("Route"),
                    Map(x => x.Description).AsSectionField<TextArea>().TextFormatIs(TextFormat.name).WithLength(300),  
            });

            AddSection().IsFramed().IsCollapsible().WithTitle("TransactionType-Channels-Fee")
                .WithColumns(new List<Column>()
                    {
                        
                      new Column(  
                     new List<IField> { 
                            Map(x => x.Type)
                                    .AsSectionField<DropDownList>()
                                    .Of(new TransactionTypeManager().GetAllTransactionType())
                                    .ListOf(x => x.Name, x => x.Id)
                                    ,
                              Map(x => x.Channel)
                                    .AsSectionField<DropDownList>()
                                    .Of(new ChannelManager().GetAllChannel())
                                    .ListOf(x => x.Name, x => x.Id),
                              Map(x => x.Fee)
                                    .AsSectionField<DropDownList>()
                                    .Of(new FeeManager().GetFees())
                                    .ListOf(x => x.Name, x => x.Id),

                                    AddSectionButton().WithText("Add Transaction_Channel_Fee")
                                        .UpdateWith(x => 
                                          {
                                                  TransactionTypeChannelFee trnx = new TransactionTypeChannelFee
                                                      {
                                                           Channel=x.Channel,
                                                           Fee=x.Fee,
                                                           TransactionType=x.Type
                                                      };
                                                 
                                               if(x.TransactionTypeChannelFees.Any(s=>s.Channel==x.Channel && s.Fee== x.Fee))
                                               {
                                                   x.TransactionTypeChannelFees.Remove(trnx);
                                               }
                                               else
                                               {
                                                   x.TransactionTypeChannelFees.Add(trnx);
                                               }
                                              return x;
                                          })
                         })
                    });
            HasMany(x => x.TransactionTypeChannelFees).As<Grid>()
                    .Of<TransactionTypeChannelFee>()
                    .WithColumn(x => x.Channel.Name, "Channel Name")
                    .WithColumn(x => x.Fee.Name, "Fee")
                    .WithColumn(x => x.TransactionType.Name, "Transaction Type")
                    .IsPaged(10);
                           

            AddButton().WithText("Save Scheme")
           .SubmitTo(x =>
           {
               bool result = false;
             object  obj = new SchemeManager().CreateScheme(x);
               if(obj!=null)
               {
                   result = true;
               }
               return result;
           })
           .OnSuccessDisplay("Successfully Saved")
           .OnFailureDisplay("Failed To save Scheme")
           .CssClassIs("btn btn-default");
        }
    }
}

