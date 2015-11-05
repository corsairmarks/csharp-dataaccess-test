namespace DataAccessTest.Web.DependencyResolution.Registry
{
    using System;
    using System.Collections.Generic;
    using DataAccessTest.Library.Value;
    using DataAccessTest.Repository;
    using DataAccessTest.Repository.NHibernate;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Mapping.ByCode;
    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    /// <summary>
    /// Configures dependency resolution for NHibernate.
    /// </summary>
    public class NHibernateRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateRegistry"/> class.
        /// </summary>
        public NHibernateRegistry()
        {
            this.IncludeRegistry<SqlDatabaseRegistry>();
            this.ForSingletonOf<Configuration>()
                .Use<Configuration>()
                .SelectConstructor(() => new Configuration());
            this.ForSingletonOf<ISessionFactory>()
                .Use(c => c.GetInstance<Configuration>().BuildSessionFactory());
            this.For<ISession>()
                .LifecycleIs<ContainerLifecycle>()
                .Use(c => c.GetInstance<ISessionFactory>().OpenSession());
            this.For<ITransaction>()
                .LifecycleIs<ContainerLifecycle>()
                .Use(c => c.GetInstance<ISession>().BeginTransaction());

            this.Scan(s =>
            {
                s.AssemblyContainingType<NHibernateDbInitializer>();
                s.AddAllTypesOf<IConformistHoldersProvider>();
            });

            For<IUnitOfWork>().LifecycleIs<ContainerLifecycle>().Use<NHibernateUnitOfWork>();

            ForSingletonOf<IDataAccessInitializer>()
                .Use<NHibernateDbInitializer>()
                .Ctor<string>()
                .Named("DefaultConnectionString")
                .Ctor<Func<IEnumerable<IConformistHoldersProvider>>>()
                .Is(c => c.GetAllInstances<IConformistHoldersProvider>);

            // one line per entity
            this.ConfigureGenericRepository<ValueModel>();
        }

        /// <summary>
        /// Configure a generic NHibernate repository for type <typeparamref name="TModel"/>.
        /// </summary>
        /// <typeparam name="TModel">The type of the repository model.</typeparam>
        private void ConfigureGenericRepository<TModel>()
            where TModel : class
        {
            this.For<NHibernateGenericRepository<TModel>>().LifecycleIs<ContainerLifecycle>();
            this.Forward<NHibernateGenericRepository<TModel>, IGenericReadRepository<TModel>>();
            this.Forward<NHibernateGenericRepository<TModel>, IGenericWriteRepository<TModel>>();
            this.Forward<NHibernateGenericRepository<TModel>, IGenericRepository<TModel>>();
        }
    }
}