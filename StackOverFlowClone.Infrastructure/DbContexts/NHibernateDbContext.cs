using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;
using StackOverFlowClone.Infrastructure.Entities;
using System.Configuration;
using NHibernate.Tool.hbm2ddl;
using NHibernate.AspNet.Identity.Helpers;
using Autofac.Core;
using Microsoft.Extensions.DependencyInjection;

namespace StackOverFlowClone.Infrastructure.DbContexts
{
    public static class NHibernateDbContext
    {
        private static ISessionFactory _session;

        public static IServiceCollection GetSession(this IServiceCollection services, string connectionString)
        {
            if (_session != null)
            {
                return services;
            }

            var myEntities = new[] {
                typeof(ApplicationUser)
            };

            // var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();

            _session = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Question>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();

            services.AddSingleton(_session);
            services.AddScoped(factory => _session.OpenSession());

            return services;
        }
    }
}