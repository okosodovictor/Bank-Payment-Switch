using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.Model
{
   public class TransactionLogModel:TransactionLog
    {
       public virtual IList<TransactionLog> transactionLogs { get; set; }
    }
}
