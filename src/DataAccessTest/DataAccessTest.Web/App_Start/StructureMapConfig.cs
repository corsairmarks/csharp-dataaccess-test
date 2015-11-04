namespace DataAccessTest.Web
{
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using DataAccessTest.Web.DependencyResolution;
    using DataAccessTest.Web.DependencyResolution.Registry;
    using Microsoft.Practices.ServiceLocation;
    using StructureMap;
    using StructureMap.Web.Pipeline;

    /// <summary>
    /// Configure the inversion of control container.
    /// </summary>
    public static class StructureMapConfig
    {
        /// <summary>
        /// Create an application container.
        /// </summary>
        /// <returns>The application-level <see cref="IContainer"/>.</returns>
        public static IContainer Initialize()
        {
            return new Container(ce =>
            {
                ConfigureDatabaseConnections(ce);
                ce.AddRegistry<DatabaseMigrationRegistry>();

                // TODO: switch between data access registries based on config
                ce.AddRegistry<EntityFrameworkRegistry>();
                ConfigureControllers(ce);
            });
        }

        /// <summary>
        /// Configures the database connection.
        /// </summary>
        /// <param name="ce">The configuration currently executing.</param>
        private static void ConfigureDatabaseConnections(ConfigurationExpression ce)
        {
            ce.ForSingletonOf<string>().Use<string>("DefaultConnection").Named("DefaultConnectionStringName");
            ce.ForSingletonOf<string>().Use(c => ConfigurationManager.ConnectionStrings[c.GetInstance<string>("DefaultConnectionStringName")].ConnectionString).Named("DefaultConnectionString");
        }

        /// <summary>
        /// Configures WebAPI controller.
        /// </summary>
        /// <param name="ce">The configuration currently executing.</param>
        private static void ConfigureControllers(ConfigurationExpression ce)
        {
            // NOTE: controllers must be Transient (the default), NOT HttpContextScoped
            ce.ForSingletonOf<StructureMapDependencyResolver>();
            ce.Forward<StructureMapDependencyResolver, IServiceLocator>();
            ce.Forward<StructureMapDependencyResolver, System.Web.Http.Dependencies.IDependencyResolver>();

            ce.ForSingletonOf<IHttpControllerActivator>().Use<StructureMapHttpControllerActivator>();

            ce.ForSingletonOf<HttpConfiguration>().Use(GlobalConfiguration.Configuration);
            ce.For<HttpContext>().LifecycleIs<HttpContextLifecycle>().Use(() => HttpContext.Current);
        }
    }
}