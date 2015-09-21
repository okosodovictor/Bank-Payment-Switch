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

namespace BankSwitch.UI.FeeManagement
{
   public class ViewFeeList:EntityUI<FeeModel>
    {
       public ViewFeeList()
       {
           AddSection()
            .WithTitle("Search")
            .IsCollapsible()
            .WithColumns(new List<Column> 
                        { 
                            new Column
                                ( new List<IField>
                                    {  
                                        Map(x => x.Name)
                                            .AsSectionField<TextBox>()
                                            .WithLength(50)      
                                            .LabelTextIs("Search for:"),
                                    }) 
                          })
                .WithFields(new List<IField>{
                   AddSectionButton() 
                        .WithText("Search")  
                        .UpdateWith (x => x)  
                      });


           //Grid Section
           HasMany(x => x.Fees).As<Grid>()
           .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.LinkEdit))
           .ApplyMod<ViewDetailsMod>(y => y
               .Popup<EditFee>("Edit Fee")
               .PrePopulate<Fee, Fee>
                   (x =>
                   {

                       return x;
                   }
                   ))
               .Of<Fee>()
               //.WithColumn(x=>x.Name) 
               .IsPaged<Model.FeeModel>(10, (x, pageDetails) =>
               {
                   int totalCount = 0;
                 x.Fees = new FeeManager().Search(x.Name, pageDetails.Start, pageDetails.Limit, out totalCount);
                   pageDetails.TotalCount = totalCount;
                   return x;
               })
              .WithRowNumbers()
              .WithColumn(x => x.Name, "Name")
              .WithColumn(x => x.FlatAmount, "Flat Amount")
              .WithColumn(x => x.PercentageOfTransaction, "Percentage Of Transcation")
              .WithColumn(x => x.Maximum, "Maximum")
              .WithColumn(x => x.Minimum, "Minimum")
              .WithAutoLoad(); 
       }
    }
}
