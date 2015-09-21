using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.Model
{
   public class TransactionTypeChannelFeeModel:TransactionTypeChannelFee
    {
       public IList<TransactionTypeChannelFee> TransactiontypeChannelFees { get; set; }
    }
}
