using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.DAO
{
   public class FeeDAO:DataRepository
    {
       public FeeDAO()
       {

       }

       public List<Core.Entities.Fee> SearchFee(string name)
       {
        List<Fee> result = new List<Fee>();
         try 
	     {	        
		 ICriteria criteria  = _Session.CreateCriteria(typeof(Fee));
                  if(string.IsNullOrEmpty(name))
                  {
                      criteria.Add(Expression.Like("Name", name.Trim(), MatchMode.Anywhere));
                  }
            result = criteria.List<Fee>() as List<Fee>;  
	      }
	    catch (Exception)
	     {
		
		  throw;
	     }
           return result;
       }

       public IList<Fee> GetAllFees()
       {

           var fees = _Session.QueryOver<Fee>().List<Fee>();
           return fees;
       }
       public IList<Fee> Search(string queryparam, int pageIndex, int pageSize, out int totalCount)
       {
           var channels = _Session.QueryOver<Fee>();
           if (!string.IsNullOrEmpty(queryparam))
           {
               channels.Where(x => x.Name.IsInsensitiveLike(queryparam, MatchMode.Anywhere)).List<Channel>();
           }
           var result = channels.Skip(pageIndex).Take(pageSize);
           totalCount = result.RowCount();
           return result.List<Fee>();
       }
    }
}
