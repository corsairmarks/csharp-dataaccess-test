namespace DataAccessTest.Web
{
    using System;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using DataAccessTest.Web.DependencyResolution;
    using DataAccessTest.Web.DependencyResolution.Registry;
    using DataAccessTest.Web.Utility;
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
        /// <param name="dataAccessType">The data access layer to use.</param>
        /// <returns>The application-level <see cref="IContainer"/>.</returns>
        public static IContainer Initialize(DataAccessType dataAccessType)
        {
            return new Container(ce =>
            {
                ce.AddRegistry<DatabaseMigrationRegistry>();

                switch (dataAccessType)
                {
                    case DataAccessType.EntityFramework:
                        ce.AddRegistry<EntityFrameworkRegistry>();
                        break;
                    case DataAccessType.PetaPoco:
                        ce.AddRegistry<PetaPocoRegistry>();
                        break;
                    case DataAccessType.NHibernate:
                        ce.AddRegistry<NHibernateRegistry>();
                        break;
                    default:
                        throw new ArgumentException(string.Format("dataAccessType \"{0}\" is not available for use", dataAccessType), "dataAccessType");
                }

                ConfigureControllers(ce);
            });
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