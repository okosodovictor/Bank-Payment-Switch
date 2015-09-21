using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitchDAO
{
   public class DataAccess
    {
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
           string connection = @"Data Source =localhost; user id= sa; password=redmond; database=SwitchDB";

           _sessionFactory = Fluently.Configure()
                  .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connection)) .Mappings(m =>{
                                         m.FluentMappings
                                         .AddFromAssemblyOf<BranchMap>();
                                     })
                                    .ExposeConfiguration(cfg => new SchemaUpdate(cfg)
                                    .Execute(false, true)).BuildConfiguration()
                                    .BuildSessionFactory();
       }
       public static ISession OpenSession()
       {
           return sessionFactory.OpenSession();
       }
            
    }
}
