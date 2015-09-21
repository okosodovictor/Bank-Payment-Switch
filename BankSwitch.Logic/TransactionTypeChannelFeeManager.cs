using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Logic
{
    public class TransactionTypeChannelFeeManager
    {
        private DataRepository _db;

        public TransactionTypeChannelFeeManager()
        {
            _db = new DataRepository();
        }

        public bool CreateTransactionTypeChannelFee( TransactionTypeChannelFee model)
        {
            bool result = false;
            try
            {
                if (model!=null)
                {
                  
                    result =  _db.Add(model);
                    _db.Commit();
                }
            }
            catch (Exception)
            {
                _db.Rollback();
                throw;
            }
            return result;
        }
        public bool Edit(TransactionTypeChannelFee model)
        {

            return _db.Update(model);
               
        }
        public IList<TransactionTypeChannelFee> RetrieveAll()
        {
            return _db.GetAll<TransactionTypeChannelFee>().ToList<TransactionTypeChannelFee>();
        }
        public IList<TransactionTypeChannelFee> Search(string name, int pageIndex, int pageSize, out int totalCount)
        {
            var query = _db.GetAll<TransactionTypeChannelFee>().ToList();
            var result = query.Skip(pageIndex).Take(pageSize);
           totalCount = result.Count();
           return result.ToList<TransactionTypeChannelFee>();
        }
    }
}
