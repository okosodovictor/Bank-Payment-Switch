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
                                        Map(x => x.Name).AsSectionField<TextBox>().WithLength(50).LabelTextIs("Name:"),
                                        Map(x => x.Code).AsSectionField<TextBox>().WithLength(50).LabelTextIs("Code:"),
                                        AddSectionButton() 
                                            .WithText("Search")  
                                            .UpdateWith (x =>
                                            {
                                                return x;
                                            })
                                    })
                          });
 

           HasMany(x => x.TransactonTypes).As<Grid>()
          .ApplyMod<IconMod>(x => x.WithIcon(Ext.Net.Icon.LinkEdit))
          .ApplyMod<ViewDetailsMod>(y => y
              .Popup<TrnsactionTypeDetail>(" TransactionType Details")
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
              .IsPaged<Model.TransactionTypeModel>(10, (x, e) =>
              {
                  int total=0;
                  x.TransactonTypes = new TransactionTypeManager().Search(x.Name, x.Code, e.Start / e.Limit, e.Limit, out total);
                  e.TotalCount = total;
                  System.Web.HttpContext.Current.Session["TotalTransactionType"] = e.TotalCount;
                  return x;
              });
      }
    }
}
