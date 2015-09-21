using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.Model
{
   public class SchemeModel:Scheme
    {
       public virtual IList<Scheme> Schemes {get; set;}
       public virtual Fee Fee { get; set; }
       public virtual Channel Channel { get; set; }
       public virtual TransactionType Type {get; set;}

    }
}
