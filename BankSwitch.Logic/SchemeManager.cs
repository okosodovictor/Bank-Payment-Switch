using BankSwitch.Core.Entities;
using BankSwitch.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Logic
{
   public class SchemeManager
    {
       private SchemeDAO _db;
       public SchemeManager()
       {
           _db = new SchemeDAO();
       }
       public object CreateScheme(Scheme model)
       {
           object result = null;
           try
           {
               var scheme = _db.Get<Scheme>().FirstOrDefault(x => x.Name == model.Name);
               if (scheme == null)
               {
                  result =  _db.Create(model);
               }
               else
               {
                   throw new Exception("This Scheme Already Exist");
               }
               if(result!=null)
               {
                   return true;
               }
           }
           catch (Exception)
           {
               _db.Rollback();
               throw;
           }
           return result;
       }

       public IList<Scheme> RetrieveAll()
       {
           return _db.GetAll<Scheme>().ToList();
       }

       public IList<Scheme> Search(string queryString, int pageIndex, int PageSize, out int totalCount)
       {
           return _db.Search(queryString, pageIndex, PageSize, out totalCount);
       }
    }
}
