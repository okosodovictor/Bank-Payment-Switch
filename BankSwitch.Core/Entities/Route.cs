using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Entities
{
   public class Route
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string CardPAN { get; set; }
        public virtual SinkNode SinkNode {get; set;}
        public virtual string Description {get; set;}
    }
}
