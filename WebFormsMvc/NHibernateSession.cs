using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;

namespace WebFormsMvc
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();

            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\hibernate.cfg.xml");

            configuration.Configure(configurationPath);

            var path = HttpContext.Current.Server.MapPath(@"~\Mappings\SampleData.hbm.xml");

            configuration.AddFile(path);

            var sessionFactory = configuration.BuildSessionFactory();

            return sessionFactory.OpenSession();
        }
    }
}