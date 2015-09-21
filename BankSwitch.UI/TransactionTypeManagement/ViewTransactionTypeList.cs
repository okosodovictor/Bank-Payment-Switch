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

namespace BankSwitch.UI.TransactionTypeManagement
{
  public  class ViewTransactionTypeList:EntityUI<TransactionTypeModel>
    {
      public ViewTransactionTypeList()
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
                                        Map(x => x.Name)
                                            .AsSectionField<TextBox>()
                                            .WithLength(50)      
                                            .LabelTextIs("Search:"),
                                        AddSectionButton() 
                                            .WithText("Search")  
                                            .UpdateWith (x => x)
                                    }) 
                          });
          //.WithFields(new List<IField>{
          //  AddSectionButton() 
          //       .WithText("Search")  
          //       .UpdateWith (x => x)  
          //     });


          //Grid Section
          HasMany(x => x.TransactonTypes).As<Grid>()
          .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.LinkEdit))
          .ApplyMod<ViewDetailsMod>(y => y
              .Popup<TrnsactionTypeDetail>("Sink Node Details")
              .PrePopulate<TransactionType, TransactionType>
                  (x =>
                  {
                      return x;
                  }
                  ))
              .Of<TransactionType>()
              .WithRowNumbers()
             .WithColumn(x => x.Name, "Name")
             .WithColumn(x => x.Code, "Code")
             .WithColumn(x => x.Description, "Description")
             .WithAutoLoad()
              .IsPaged<Model.TransactionTypeModel>(10, (x, pageDetails) =>
              {
                  int totalCount;
                  x.TransactonTypes = new TransactionTypeManager().Search(x.Name, pageDetails.Start, pageDetails.Limit, out totalCount);
                  pageDetails.TotalCount = totalCount;
                  return x;
              });
      }
    }
}
