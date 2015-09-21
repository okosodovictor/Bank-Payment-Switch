using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.Model
{
   public class ChannelModel:Channel
    {
       public virtual IList<Channel> Channels { get; set;}
    }
}
