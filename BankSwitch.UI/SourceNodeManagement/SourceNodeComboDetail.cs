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

namespace BankSwitch.UI.SourceNodeManagement
{
  public  class SourceNodeComboDetail:EntityUI<SchemeModel>
    {
      public SourceNodeComboDetail()
      {
          WithTitle("Source Management");

          AddSection().WithTitle("List Of Combos").IsFramed().IsCollapsible()
         .WithColumns(new List<Column>()
            {
                new Column(new List<IField>()
                {
                         HasMany(x => x.TransactionTypeChannelFees)
                        .AsSectionField<Grid>() 
                        .Of<TransactionTypeChannelFee>()
                        .WithRowNumbers()
                        .WithColumn(x => x.Channel.Name,"Channel")
                        .WithColumn(x => x.Fee.Name,"Fee")
                        .WithColumn(x => x.TransactionType.Name,"Type")
                        .WithRowNumbers()
                        .IsPaged<SchemeModel>(10, (x, e) =>
                        {
                            x.Schemes = new SchemeManager().RetrieveAll();
                            return x;
               }).ApplyMod<ViewDetailsMod>(y => y.Popup<SourceNodeSchemeComboDetails>("View Details"))    
               })
            }
          );
      }
    }
}
