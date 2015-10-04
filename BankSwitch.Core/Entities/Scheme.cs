using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BankSwitch.Core.Entities
{
  public class Scheme
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Route Route { get; set; }
        public virtual IList<TransactionTypeChannelFee> TransactionTypeChannelFees { get; set; }
        public virtual string Description { get; set; }

        public virtual Fee FeeUI { get; set; }
        public virtual Channel ChannelUI { get; set; }
        public virtual TransactionType TypeUI { get; set; }
        public virtual IList<Scheme> Schemes { get; set; }
        public Scheme()
        {
            TransactionTypeChannelFees = new List<TransactionTypeChannelFee>();
        }
 
    }
}
