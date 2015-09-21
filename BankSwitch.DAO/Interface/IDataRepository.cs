using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Interfaces
{
   public interface IDataRepository
    {
        IList<T> GetAll<T>() where T : class;
        T GetById<T>(int id) where T : class;
        IQueryable<T> Get<T>() where T : class;
        bool Add<T>(T entity) where T : class;
        bool Add<T>(IEnumerable<T> items) where T : class;
        bool Update<T>(T entity) where T : class;
        T FindBy<T>(T id) where T : class;
        void Commit();
        void BeginTransaction();
        void Rollback(); 
    }
}
