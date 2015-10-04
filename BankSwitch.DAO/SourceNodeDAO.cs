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
using System.Collections;

namespace BankSwitch.Core.DAO
{
    public class SourceNodeDAO : DataRepository
    {
        public SourceNodeDAO()
        {

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
        public object Edit(SourceNode model)
        {
            object result = null;
            using (var session = DataAccess.DataAccess.OpenSession())
            {
                using (var transactn = session.BeginTransaction())
                {
                    session.Merge(model);
                    transactn.Commit();
                }
            }
            return result;
        }
        public IList<SourceNode> SearchSinkNode(string name, string hostName, string iPAddress, string port, int start, int limit, out int total)
        {

            List<SourceNode> result = new List<SourceNode>();
            try
            {
                ICriteria criteria = _Session.CreateCriteria(typeof(SourceNode));
                if (!string.IsNullOrEmpty(name))
                {
                    criteria.Add(Expression.Like("Name", name));
                }
                if (!string.IsNullOrEmpty(hostName))
                {
                    criteria.Add(Expression.Like("HostName", hostName));
                }
                if (!string.IsNullOrEmpty(iPAddress))
                {
                    criteria.Add(Expression.Like("IPAddress", iPAddress));
                }
                if (!string.IsNullOrEmpty(port))
                {
                    criteria.Add(Expression.Like("Port", port));
                }
                ICriteria countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(Projections.RowCountInt64());
                ICriteria listCriteria = CriteriaTransformer.Clone(criteria).SetFirstResult(start).SetMaxResults(limit);
                listCriteria.AddOrder(Order.Desc("Id"));

                IList allResults = _Session.CreateMultiCriteria().Add(listCriteria).Add(countCriteria).List();

                foreach (var o in (IList)allResults[0])
                {
                    result.Add((SourceNode)o);
                }

                total = Convert.ToInt32((long)((IList)allResults[1])[0]);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
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

        public object UpdateSourceNode(SourceNode sourceNode)
        {
            object result = false;
            using (var session = DataAccess.DataAccess.OpenSession())
            {
                using (var transactn = session.BeginTransaction())
                {
                    try
                    {
                        result = session.Merge(sourceNode);
                        transactn.Commit();
                    }
                    catch (Exception)
                    {
                        transactn.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }

    }
}
