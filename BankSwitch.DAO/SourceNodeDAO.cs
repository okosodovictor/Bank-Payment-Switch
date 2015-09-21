using BankSwitch.Core.DataAccess;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate;

namespace BankSwitch.Core.DAO
{
    public class SourceNodeDAO : DataRepository
    {
        public SourceNodeDAO()
        {

        }
        public object Create(SourceNode model)
        {
            object result = null;
            try
            {
                using (ISession session = _Session.SessionFactory.OpenSession())
                {
                    using (ITransaction trn =_Session.BeginTransaction())
                    {
                        SourceNode sourceNode = new SourceNode
                       {
                           Name = model.Name,
                           Port = model.Port,
                           IsActive = false,
                           IPAddress = model.IPAddress,
                           HostName = model.HostName
                       };
                    result = session.Save(sourceNode);

                    sourceNode.Schemes = model.Schemes;

                    foreach (var scheme in model.Schemes)
                    {
                        session.Update(scheme);
                        trn.Commit();
                    }

                    }
                }
                return result;
            }
            catch (Exception)
            {
                _Session.Transaction.Rollback();
                throw;
            }
        }

        public object Save(SourceNode model)
        {
            object result = null;
            using (var session = DataAccess.DataAccess.OpenSession())
            {
                using (var transactn = session.BeginTransaction())
                {
                  result = session.Save(model);
                    transactn.Commit();
                }
            }
            return result;
        }
        public IList<SourceNode> SearchSinkNode(string queryparam, int pageIndex, int pageSize, out int totalCount)
        {
           
            var query = _Session.QueryOver<SourceNode>();

            if(string.IsNullOrEmpty(queryparam))
            {
                totalCount = query.RowCount();
               var sourceNodes = query.List<SourceNode>();
               return sourceNodes;
            }
            else
            {
                query.Where(x => x.Name.IsLike(queryparam, MatchMode.Anywhere) ||
                    x.HostName.IsLike(queryparam, MatchMode.Anywhere) ||
                    x.IPAddress.IsLike(queryparam, MatchMode.Anywhere) ||
                    x.Port.IsLike(queryparam, MatchMode.Anywhere)
                )
                  .List<SinkNode>();
            }

            var result = query.Skip(pageIndex).Take(pageSize);
            totalCount = result.RowCount();
            return result.List<SourceNode>();
        }

        public SourceNode GetByName(string name)
        {
            try
            {
                var query = _Session.QueryOver<SourceNode>()
                       .Where(x => x.Name == name)
                       .SingleOrDefault();
                return query;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public IList<SourceNode> Retrieve()
        {
            var query = _Session.QueryOver<SourceNode>().List<SourceNode>();
            return query;
        }

    }
}
