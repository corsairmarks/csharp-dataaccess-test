﻿namespace DataAccessTest.Web
{
    using System;
    using System.Configuration;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Dispatcher;
    using System.Web.Mvc;
    using System.Web.Routing;
    using DataAccessTest.Repository;
    using DataAccessTest.Web.Utility;
    using FluentMigrator.Runner;
    using Microsoft.Practices.ServiceLocation;
    using StructureMap.Web.Pipeline;

    /// <summary>
    /// The entry point for the <see cref="HttpApplication"/>.
    /// </summary>
    public class WebApiApplication : HttpApplication
    {
        /// <summary>
        /// Application startup hook.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var dataAccess = ConfigurationManager.AppSettings["DataAccess"];
            DataAccessType dat;
            var isDefinedDataAccessType = Enum.TryParse(dataAccess, out dat) && Enum.IsDefined(dat.GetType(), dat);
            var container = StructureMapConfig.Initialize(isDefinedDataAccessType
                ? dat
                : DataAccessType.EntityFramework);
            container.GetInstance<IMigrationRunner>().MigrateUp();
            var dataAccessInitializer = container.TryGetInstance<IDataAccessInitializer>();
            if (dataAccessInitializer != null)
            {
                dataAccessInitializer.Initialize();
            }

            // MVC controller injection.
            DependencyResolver.SetResolver(container.GetInstance<IServiceLocator>());

            // WebAPI controller injection.
            GlobalConfiguration.Configuration.DependencyResolver = container.GetInstance<System.Web.Http.Dependencies.IDependencyResolver>();
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), container.GetInstance<IHttpControllerActivator>());
        }

        /// <summary>
        /// Application request end hook.
        /// </summary>
        protected void Application_EndRequest()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }
    }
}