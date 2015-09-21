using BankSwitch.Core.DAO;
using BankSwitch.Core.Entities;
using BankSwitch.Core.Interfaces;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Managers
{
    public class SourceNodeManager
    {
        private SourceNodeDAO _db;
        public SourceNodeManager(SourceNodeDAO db)
        {
            _db = db;
        }
        public SourceNodeManager()
        {
            _db = new SourceNodeDAO();
        }
        public bool AddSourceNode(SourceNode sourceNode)
        {
            bool result = false;
            var query = _db.GetAll<SourceNode>().FirstOrDefault(x => x.IPAddress == sourceNode.IPAddress);
            if(query!=null)
            {
                throw new Exception("Source Node with this IP Already Exist");
            }
            else
            {
                result = _db.Add(sourceNode);
            }
            return result;
        }
        public IList<SourceNode> GetAllSourceNode()
        {
            var query = _db.GetAll<SourceNode>().ToList();
            return query;
        }
        public IList<SourceNode> Search(string querystring, int pageIndex, int pageSize, out int totalCount)
        {
            var query = new List<SourceNode>();
            if (string.IsNullOrEmpty(querystring))
            {
                pageIndex = 0;
                pageSize = 0;
                totalCount = 0;
            }
            else
            {
                query = _db.GetAll<SourceNode>()
                   .Where(x => x.Name.IsInsensitiveLike(querystring, MatchMode.Anywhere) ||
                       x.HostName.IsInsensitiveLike(querystring, MatchMode.Anywhere) ||
                       x.IPAddress.IsInsensitiveLike(querystring, MatchMode.Anywhere) ||
                       x.Port.IsInsensitiveLike(querystring, MatchMode.Anywhere)
                       ).ToList();

            }
            var result = query.Skip(pageIndex).Take(pageSize);
            totalCount = query.Count;
            return result.ToList();
        }
        public SourceNode Find(int id)
        {
            var query = _db.GetById<SourceNode>(id);
            return query;
        }
    }
}
