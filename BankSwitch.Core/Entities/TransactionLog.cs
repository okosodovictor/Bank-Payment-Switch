using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Entities
{
   public class TransactionLog
    {
        public virtual int Id { get; set; }
        public virtual string MTI { get; set; }
        public virtual string CardPAN { get; set; }
        public virtual string STAN { get; set; }
        public virtual DateTime TransactionDate { get; set; }
        public virtual string Account1 { get; set; }
        public virtual string Account2 { get; set; }
        public virtual string TransactionType { get; set; }
        public virtual string Channel { get; set; }
        public virtual string SourceNode { get; set; }
        public virtual string SinkNode { get; set; }
        public virtual string Route { get; set; }
        public virtual string Scheme { get; set; }
        public virtual double Amount { get; set; }
        public virtual string Fee { get; set; }
        public virtual string ResponseCode { get; set; }
        public virtual string ResponseDescription { get; set; }
        public virtual decimal Charge { get; set; }
        public virtual bool IsReversePending { get; set; }
        public virtual bool IsReversed { get; set; }
        public virtual string OriginalDataElement { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateModified { get; set; }
    }
}
