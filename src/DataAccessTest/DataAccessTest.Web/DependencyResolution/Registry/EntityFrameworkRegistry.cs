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
    using DataAccessTest.Library.Value;
    using DataAccessTest.Repository;
    using DataAccessTest.Repository.EntityFramework;
    using DataAccessTest.Repository.EntityFramework.Sample;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.Pipeline;

    /// <summary>
    /// Configures dependency resolution for Entity Framework.
    /// </summary>
    public class EntityFrameworkRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkRegistry"/> class.
        /// </summary>
        public EntityFrameworkRegistry()
        {
            this.ForSingletonOf<IDbConnectionFactory>().Use<SqlConnectionFactory>().Ctor<string>().Named("DefaultConnectionString");
            this.For<DbConnection>()
                .LifecycleIs<ContainerLifecycle>()
                .Use(c => c.GetInstance<IDbConnectionFactory>().CreateConnection(c.GetInstance<string>("DefaultConnectionString")));
            this.ForSingletonOf<string>().Use<string>("System.Data.SqlClient").Named("DbProviderInvariantName");
            this.ForSingletonOf<DbProviderFactory>().Use(SqlClientFactory.Instance);
            this.ForSingletonOf<DbProviderServices>().Use(SqlProviderServices.Instance);

            this.ForSingletonOf<DbConfiguration>()
                .Use<EntityFrameworkConfig<SampleDbContext>>()
                .Ctor<string>()
                .Named("DbProviderInvariantName");

            this.Scan(s =>
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
            this.ConfigureGenericRepository<SampleDbContext, ValueModel>();
        }

        /// <summary>
        /// Configure a generic Entity Framework repository for type <typeparamref name="TModel"/> in the the <typeparamref name="TContext"/>.
        /// </summary>
        /// <typeparam name="TContext">The type of the Entity Framework context containing the data set.</typeparam>
        /// <typeparam name="TModel">The type of the repository model.</typeparam>
        private void ConfigureGenericRepository<TContext, TModel>()
            where TContext : DbContext
            where TModel : class
        {
            this.For<EntityFrameworkGenericRepository<TContext, TModel>>().LifecycleIs<ContainerLifecycle>();
            this.Forward<EntityFrameworkGenericRepository<TContext, TModel>, IGenericReadRepository<TModel>>();
            this.Forward<EntityFrameworkGenericRepository<TContext, TModel>, IGenericWriteRepository<TModel>>();
            this.Forward<EntityFrameworkGenericRepository<TContext, TModel>, IGenericRepository<TModel>>();
        }

        /// <summary>
        /// Register all instances of <see cref="DbContext"/> with the <see cref="ContainerLifecycle"/>.
        /// </summary>
        private class DbContextConvention : IRegistrationConvention
        {
            /// <summary>
            /// Processes each item in an assembly scan.
            /// </summary>
            /// <param name="type">The type currently being read by the scanner.</param>
            /// <param name="registry">The registry to modify.</param>
            public void Process(Type type, Registry registry)
            {
                if (!type.IsAbstract && !type.IsInterface && type.IsSubclassOf(typeof(DbContext)))
                {
                    registry.For(type).LifecycleIs(Lifecycles.Container).Use(type);
                }
            }
        }
    }
}