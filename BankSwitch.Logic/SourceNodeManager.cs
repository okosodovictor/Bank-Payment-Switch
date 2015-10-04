using BankSwitch.Core.DAO;
using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Logic
{
    public class SourceNodeManager
    {
        public SourceNodeDAO _db;
        public SourceNodeManager()
        {
            _db = new SourceNodeDAO();
        }
        public object CreateSourceNode(SourceNode model)
        {
            object result = null;
            try
            {

                result = _db.Save(model);
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public bool EditSourceNode(SourceNode model)
        {
            bool result = false;
            try
            {
                object obj = _db.Edit(model);
                if (obj != null)
                {
                    result = true;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public IList<SourceNode> RetrieveAll()
        {
            return _db.Retrieve();
        }
        public IList<SourceNode> GetAllSourceNode(string name, string hostName, string iPAddress, string port, int start, int limit, out int total)
        {
            return _db.SearchSinkNode(name, hostName, iPAddress, port, start, limit, out total);
        }

        public SourceNode GetByID(int sourceID)
        {
            var sourceNode = _db.GetById<SourceNode>(sourceID);
            return sourceNode;
        }

        public void Update(SourceNode model)
        {
            using (var session = DataAccess.OpenSession())
            {
                using (var transactn = session.BeginTransaction())
                {
                     session.Merge(model);
                    transactn.Commit();
                }
            }
        }
    }
}
