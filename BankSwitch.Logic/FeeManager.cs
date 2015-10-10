using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSwitch.DAO;
using BankSwitch.Core.Entities;

namespace BankSwitch.Logic
{
   public class FeeManager
    {
       private FeeDAO _db;
       public FeeManager()
       {
           _db = new FeeDAO();
       }

       public bool CreateFee(Fee model)
       {
           bool result =false;
           try
           {
               var fee = _db.Get<Fee>().FirstOrDefault(x => x.Name == model.Name);
               if(fee==null)
               {
                   fee = new Fee
                   {
                        Name=model.Name,
                        FlatAmount=model.FlatAmount,
                        PercentageOfTransaction= model.PercentageOfTransaction,
                        Maximum= model.Maximum,
                        Minimum= model.Minimum
                   };
                  result = _db.Add(fee);
               }
               else
               {
                   throw new Exception("Fee With this Name Already Exist");
               }
               return result;
           }
           catch (Exception)
           {
               _db.Rollback();
               throw;
           }
       }

       public bool EditFee(Fee model)
       {
           bool result = false;
           try
           {
               var fee = _db.Get<Fee>().FirstOrDefault(x => x.Id==model.Id);
               if (fee != null)
               {
                   fee.Name = model.Name;
                   fee.FlatAmount = model.FlatAmount;
                   fee.PercentageOfTransaction = model.PercentageOfTransaction;
                   fee.Maximum = model.Maximum;
                   fee.Minimum = model.Minimum;
                   result = _db.Update(fee);
                   _db.Commit();
               }
               else
               {
                   throw new Exception("This Fee Does Not Exist");
               }
               return result;
           }
           catch (Exception ex)
           {
               _db.Rollback();
               throw;
           }
       }

       public IList<Fee> GetFees()
       {
           var fees = _db.GetAllFees();
           return fees;
       }
       public IList<Fee> SearchFee(string name)
       {
           return _db.SearchFee(name);
       }

       public IList<Fee> Search(string queryparam, int pageIndex, int pageSize, out int totalCount)
       {
           return _db.Search(queryparam, pageIndex, pageSize, out totalCount);
       }
    }
}
