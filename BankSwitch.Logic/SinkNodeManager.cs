using BankSwitch.Core.DAO;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Logic
{
   public class SinkNodeManager
    {
        private SinkNodeDAO _db;
        public SinkNodeManager(SinkNodeDAO db)
        {
            _db = db;
        }
        public SinkNodeManager()
        {
            _db = new SinkNodeDAO();
        }
        public bool AddSinkNode(SinkNode sinkNode)
        {
            bool result = false;
            try
            {
                var node = _db.GetAll<SinkNode>().FirstOrDefault(x => x.IPAddress == sinkNode.IPAddress && x.Port==sinkNode.Port);
                if (node != null)
                {
                    throw new Exception("This Sink Node With this IP Address and Port already Exist");
                }
                else
                {
                    result = _db.Add(sinkNode);
                }
                return result;
            }
            catch (Exception ex)
            {
                _db.Rollback();
                throw ex;
            }
        }
        public bool Edit(SinkNode model)
        {
            try
            {
                bool result = false;
                var sinkNode = _db.GetAll<SinkNode>().FirstOrDefault(x => x.IPAddress == model.IPAddress);
                if (sinkNode != null)
                {
                    sinkNode.Name = model.Name;
                    sinkNode.HostName = model.HostName;
                    sinkNode.IPAddress = model.IPAddress;
                    sinkNode.Port = model.Port;
                     sinkNode.IsActive = model.IsActive;
                    result = _db.Update(sinkNode);
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

       public IList<SinkNode> GetSinkNodes(string name, string hostName, string iPAddress)
       {
           return _db.GetSinkNodes(name, hostName, iPAddress);
       }
       public IList<SinkNode> GetAllSinkNode()
       {
           var query = _db.GetAllSinkNodes();
           return query;
       }

       public SinkNode GetById(int ID)
       {
           var sinkNode = _db.GetById<SinkNode>(ID);
           return sinkNode;
       }

       public void Update(SinkNode sinkNode)
       {
           if(sinkNode!=null)
           {
               _db.Update(sinkNode);
           }
       }
    }
}
