using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using BankSwitch.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.TransactionTypeChannelfeeManagement
{
   public class ViewTransactionTypeChannelFee:EntityUI<TransactionTypeChannelFeeModel>
    {
       public ViewTransactionTypeChannelFee()
       {
           //Search 
           AddSection()
            .WithTitle("Search")
            .IsCollapsible()
            .WithColumns(new List<Column> 
                        { 
                            new Column
                                ( new List<IField>
                                    {  
                                        //Map(x => x.Name)
                                        //    .AsSectionField<TextBox>()
                                        //    .WithLength(50)      
                                        //    .LabelTextIs("Search for:"),
                                    }) 
                          })
                .WithFields(new List<IField>{
                   AddSectionButton() 
                        .WithText("Search")  
                       .UpdateWith(x =>
                                    {
                                        return x;
                                    })
                  
                                    });


           //Grid Section
           HasMany(x =>x.TransactiontypeChannelFees).As<Grid>()
           .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.LinkEdit))
           .ApplyMod<ViewDetailsMod>(y => y
               .Popup<EditTransactionTypeChannelFee>("View Transaction Combo")
               .PrePopulate<TransactionTypeChannelFee, TransactionTypeChannelFee>
                   (x =>
                   {
                       return new TransactionTypeChannelFee
                       {
                           Id = x.Id,
                           TransactionType = x.TransactionType,
                           Channel = x.Channel,
                           Fee = x.Fee,
                       };
                   }
                   ))
               .Of<TransactionTypeChannelFee>()
               //.WithColumn(x=>x.Name) 
               .IsPaged<TransactionTypeChannelFeeModel>(10, (x, pageDetails) =>
               {
                    int totalCount;
                    x.TransactiontypeChannelFees = new TransactionTypeChannelFeeManager().Search("", pageDetails.Start, pageDetails.Limit, out totalCount);
                   pageDetails.TotalCount = totalCount;
                   return x;
               })
              .WithRowNumbers()
              .WithColumn(x => x.TransactionType.Name, "Transaction Type")
              .WithColumn(x => x.Channel.Name, "Channel")
              .WithColumn(x => x.Fee.Name, "Fee")
              .WithAutoLoad();
       }
    }
}
