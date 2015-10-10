using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using BankSwitch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Logic
{
   public class TransactionTypeManager
    {
       private TransactionTypeDAO _db;
       public TransactionTypeManager(TransactionTypeDAO db)
       {
           db = _db;
       }
       public TransactionTypeManager()
       {
           _db = new TransactionTypeDAO();
       }
       public bool AddTransactionType(TransactionType model)
       {
           var check = _db.Get<TransactionType>().FirstOrDefault(x => x.Name == model.Name && x.Code == model.Code);
           if (check != null)
           {
               throw new Exception("This TransactionType already Exist");
           }
           else
           {
               return _db.Add(new TransactionType
               {
                   Code = model.Code,
                   Name = model.Name,
                   Description = model.Description
               });
           }
       }
       public bool Edit(TransactionType model)
       {
           bool result = false;
           try
           {
               var transx = _db.Get<TransactionType>().FirstOrDefault(x =>x.Id==model.Id);
               if (transx != null)
               {
                   transx.Code = model.Code;
                   transx.Description = model.Description;
                   transx.Name = model.Name;
                  result =_db.Update(transx);
                   _db.Commit();
               }
               return result;
           }
           catch (Exception ex)
           {
               _db.Rollback();
               throw ex;
           } 
       }

       public IList<TransactionType> GetAllTransactionType()
       {
          return  _db.GetAll<TransactionType>().ToList<TransactionType>();     
       }
       public TransactionType GetByName(string name)
       {
         var trnx = new TransactionType();
         if(string.IsNullOrEmpty(name))
         {
             return trnx;
         }
         else
         {
           trnx = _db.Get<TransactionType>().FirstOrDefault(x=>x.Name==name);
           return trnx;
         }
       }

       public IList<TransactionType> Search(string name,string code, int start, int limit, out int total)
       {
           return _db.Search(name, code, start, limit, out total);
       }

       public TransactionType GetByCode(string transactionTypeCode)
       {
           var trxType = _db.Get<TransactionType>().Where(x => x.Code == transactionTypeCode).FirstOrDefault();
           return trxType;
       }
    }
}
