using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Entities
{
   public class SinkNode
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string HostName { get; set; }
        public virtual string IPAddress { get; set; }
        public virtual string Port { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
