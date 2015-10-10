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
   public class ViewTransactionLog:EntityUI<TransactionLogModel>
    {
       public ViewTransactionLog()
       {
           UseFullView();
            WithTitle("View TransactionLog");
            AddNorthSection()
               .WithTitle("TransactionLog Search")
               .IsCollapsible()
               .IsFramed()
               .WithColumns(
                   new List<Column>()
                    {
                        new Column(new List<IField>()
                        {  
                           Map(x => x.TransactionDate).AsSectionField<DateField>().LabelTextIs("Transaction Date From"),
                           Map(x => x.CardPAN).AsSectionField<TextBox>().WithLength(30),
                              AddSectionButton()
                            .WithText("Search")
                            .UpdateWith(x=> 
                                {
                                    return x;
                                })
                            }),
                              new Column(
                            new List<IField>()
                            {
                                Map(x => x.TransactionDate).AsSectionField<DateField>().LabelTextIs("Transaction Date To"),
                               Map(x=>x.ResponseCode).AsSectionField<TextBox>().WithLength(30),
                            }),
                                new Column(
                            new List<IField>()
                            {
                               Map(x=>x.MTI).AsSectionField<TextBox>().WithLength(30),
                            }),
                    });

            Map(x => x.transactionLogs)
            .AsCenter<Grid>()
            .ApplyMod<ExportMod>(x => x.ExportToExcel().ExportToCsv().SetFileName("List Of Transaction Log")
             .ExportAllRows()
             .SetPagingLimit<TransactionLogModel>(y => (int)System.Web.HttpContext.Current.Session["TransactionLogTotalCount"]))
             .ApplyMod<ViewDetailsMod>(mod => mod.Popup<TransactionLogDetail>("Details"))
            .Of<TransactionLog>()
            .WithRowNumbers()
            .WithColumn(x => x.Account1)
            .WithColumn(x => x.Account2)
            .WithColumn(x=>x.Amount)
            .WithColumn(x=>x.CardPAN)
            .WithColumn(x=>x.MTI)
            .WithColumn(x=>x.Channel)
            .WithColumn(x=>x.Charge)
            .WithColumn(x => x.TransactionDate,"Transaction Date")
            .WithColumn(x=>x.ResponseDescription,"Response Description")
            .WithColumn(x=>x.Route)
            .WithColumn(x=>x.Scheme)
            .WithColumn(x=>x.SinkNode)
            .WithColumn(x=>x.SourceNode)
            .WithColumn(x=>x.TransactionType)
            .WithColumn(x=>x.STAN)
            .WithSingleSelection()
            .IsPaged<TransactionLogModel>(10, (x, c) =>
            {

                int total = 0;
                x.transactionLogs = new TransactionLogManager().GetAllTransactionLog(x.CardPAN, x.MTI, x.ResponseCode, x.TransactionDate, x.TransactionDate, c.Start / c.Limit, c.Limit, out total);
                c.TotalCount = total;
                System.Web.HttpContext.Current.Session["TransactionLogTotalCount"] = c.TotalCount;
                return x;
            });

        }
   }
}
