namespace DataAccessTest.Web.DependencyResolution.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.SqlServer;
    using System.Data.SqlClient;
    using System.Reflection;
    using DataAccessTest.Library.Value;
    using DataAccessTest.Repository;
    using DataAccessTest.Repository.EntityFramework;
    using DataAccessTest.Repository.EntityFramework.Sample;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Pipeline;

    public class EntityFrameworkRegistry : Registry
    {
        public EntityFrameworkRegistry()
        {
            ForSingletonOf<IDbConnectionFactory>().Use<SqlConnectionFactory>().Ctor<string>().Named("DefaultConnectionString");
            For<DbConnection>()
                .LifecycleIs<ContainerLifecycle>()
                .Use(c => c.GetInstance<IDbConnectionFactory>().CreateConnection(c.GetInstance<string>("DefaultConnectionString")));
            ForSingletonOf<string>().Use<string>("System.Data.SqlClient").Named("DbProviderInvariantName");
            ForSingletonOf<DbProviderFactory>().Use(SqlClientFactory.Instance);
            ForSingletonOf<DbProviderServices>().Use(SqlProviderServices.Instance);

            ForSingletonOf<DbConfiguration>()
                .Use<EntityFrameworkConfig<SampleDbContext>>()
                .Ctor<string>()
                .Named("DbProviderInvariantName");

            Scan(s =>
            {
                s.AssemblyContainingType<EntityFrameworkDbInitializer>();
                s
                    .ConnectImplementationsToTypesClosing(typeof(IDatabaseInitializer<>))
                    .OnAddedPluginTypes(gfe => gfe.LifecycleIs(Lifecycles.Container));
                s.Convention<DbContextConvention>();
            });

            ForSingletonOf<IEnumerable<IDatabaseInitializer<DbContext>>>().Use("blah", c => c.GetAllInstances<IDatabaseInitializer<DbContext>>());

            For<IUnitOfWork>().LifecycleIs<ContainerLifecycle>().Use<EntityFrameworkUnitOfWork<SampleDbContext>>();

            ForSingletonOf<IDataAccessInitializer>()
                .Use<EntityFrameworkDbInitializer>()
                .Ctor<Func<DbConfiguration>>()
                .Is(c => c.GetInstance<DbConfiguration>)
                .Ctor<Func<IEnumerable<DbContext>>>()
                .Is(c => c.GetAllInstances<DbContext>);

            // one line per entity
            ConfigureGenericRepository<SampleDbContext, ValueModel>();
        }

        private class DbContextConvention : IRegistrationConvention
        {
            public void Process(Type type, Registry registry)
            {
                if (!type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(DbContext)))
                {
                    registry.For(type).LifecycleIs(Lifecycles.Container).Use(type);
                }
            }
        }

        private void ConfigureGenericRepository<TContext, TModel>()
            where TContext : DbContext
            where TModel : class
        {
            For<EntityFrameworkGenericRepository<TContext, TModel>>().LifecycleIs<ContainerLifecycle>();
            Forward<EntityFrameworkGenericRepository<TContext, TModel>, IGenericReadRepository<TModel>>();
            Forward<EntityFrameworkGenericRepository<TContext, TModel>, IGenericWriteRepository<TModel>>();
            Forward<EntityFrameworkGenericRepository<TContext, TModel>, IGenericRepository<TModel>>();
        }
    }
}