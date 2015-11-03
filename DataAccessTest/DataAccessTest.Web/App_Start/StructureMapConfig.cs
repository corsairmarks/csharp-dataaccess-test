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
            }); ;
        }

        private static void ConfigureDatabaseConnections(ConfigurationExpression ce)
        {
            ce.ForSingletonOf<string>().Use<string>("DefaultConnection").Named("DefaultConnectionStringName");
            ce.ForSingletonOf<string>().Use(c => ConfigurationManager.ConnectionStrings[c.GetInstance<string>("DefaultConnectionStringName")].ConnectionString).Named("DefaultConnectionString");
        }

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