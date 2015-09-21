using BankSwitch.Core.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.DataAccess
{
   public class DataAccess
    {
        private static ISessionFactory factory { get; set; }

        private static ISession contextSession { get; set; }

       private static ISessionFactory _sessionFactory;

        private static ISessionFactory sessionFactory
        {
            get
            {
               if(_sessionFactory == null)
               {
                   InitializeSessionFactory();
               }
               return _sessionFactory;
           }
       }
       private static void InitializeSessionFactory()
       {
           string connectionstring = @"Data Source =localhost; user id= sa; password=redmond; database=SwitchDB";

           _sessionFactory = Fluently.Configure()
                  .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionstring)).Mappings(m =>{
                                         m.FluentMappings
                                         .AddFromAssemblyOf<RouteMap>();
                                     })
                                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg)
                                    .Execute(false, true)).BuildConfiguration()
                                    .BuildSessionFactory();
       }

       public ISession GetSession()
       {

           if (CallContext.GetData("sessionItem") == null)
           {
               InitializeSessionFactory();
               contextSession = factory.OpenSession();
               contextSession.BeginTransaction();
               CallContext.SetData("sessionItem", contextSession);
           }

           contextSession = (ISession)CallContext.GetData("sessionItem");
           if (contextSession.IsOpen != true)
           {
               InitializeSessionFactory();
               contextSession = factory.OpenSession();
               contextSession.BeginTransaction();
               CallContext.SetData("sessionItem", contextSession);
           }

           return (ISession)CallContext.GetData("sessionItem");
           //}

       }
       public static ISession OpenSession()
       {
           return sessionFactory.OpenSession();
       }            
    }
}
