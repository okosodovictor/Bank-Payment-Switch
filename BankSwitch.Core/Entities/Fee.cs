using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Entities
{
   public class Fee
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual double FlatAmount { get; set; }
        public virtual double PercentageOfTransaction { get; set; }
        public virtual double Maximum { get; set; }
        public virtual double Minimum { get; set; }
    }
}
