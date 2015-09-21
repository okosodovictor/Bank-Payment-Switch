using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.Model
{
   public class SourceNodeModel:SourceNode
    {
       public virtual IList<SourceNode> SourceNodes { get; set;}
 
    }
}
