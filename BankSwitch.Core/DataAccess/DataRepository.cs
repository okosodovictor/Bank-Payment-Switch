using BankSwitch.Core.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Linq;

namespace BankSwitch.Core.DataAccess
{
   public class DataRepository:IDataRepository, IDisposable
    {
        private ISession _session;
        protected ISession _Session
        {
            get
            {
                if (_session == null)
                {
                    _session = DataAccess.OpenSession();
                    return _session;
                }
                else
                {
                    return _session;
                }
            }
            set
            {
                _session = value;
            }
        }
        protected ITransaction _transaction = null;
        public DataRepository()
        {
            _Session = DataAccess.OpenSession();

        }

        public DataRepository(ISession sesson)
        {
            _Session = sesson;
        }

        public void Commit()
        {
            if (_Session.Transaction.IsActive)
            {
                _Session.Transaction.Commit();
            }
        }
        public void Rollback()
        {
            if (_Session.Transaction.IsActive)
            {
                _Session.Transaction.Rollback();
                _Session.Clear();
            }
        }
        public void BeginTransaction()
        {
            Rollback();
            _Session.BeginTransaction();
        }
        public void CloseTransaction()
        {
            _transaction.Dispose();
            _transaction = null;
        }

        private void CloseSession()
        {
            _Session.Close();
            _Session.Dispose();
            _Session = null;
        }
        public void Dispose()
        {
            if (_transaction != null)
            {
            
                Commit();
            }

            if (_Session != null)
            {
                _Session.Flush(); // commit session transactions
                CloseSession();
            }
        }

        public IQueryable<T> Get<T>() where T : class
        {
            return _Session.Query<T>();
        }
        public T GetById<T>(int id) where T : class
        {
            return _Session.Get<T>(id);

        }

        public IList<T> GetAll<T>() where T : class
        {
            return _Session.QueryOver<T>().List();

        }

        public bool Add<T>(T entity) where T : class
        {
            _Session.Save(entity);
            return true;
        }
        public T FindBy<T>(T id) where T : class
        {
            _Session.CacheMode = CacheMode.Normal;
            return _Session.Get<T>(id);
        }

        public bool Add<T>(IEnumerable<T> items) where T : class
        {
            foreach (T item in items)
            {
                _Session.Save(item);
            }
            return true;
        }

        public bool Update<T>(T entity) where T : class
        {
            _Session.Update(entity);
            _Session.Flush();
            return true;
        }
    }
}
