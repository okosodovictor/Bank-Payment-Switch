using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Entities
{
   public class TransactionTypeChannelFee
    {
        public virtual int Id { get; set; }
        public virtual TransactionType TransactionType { set; get; }
        public virtual Channel Channel { set; get; }
        public virtual Fee Fee { set; get; }
        public virtual Scheme Scheme { get; set; }
    }
}
