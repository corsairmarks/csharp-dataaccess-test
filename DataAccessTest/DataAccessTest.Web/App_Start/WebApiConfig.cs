namespace DataAccessTest.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    /// <summary>
    /// WebAPI route configuration for the application.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Configure the WebAPI routes for the application.
        /// </summary>
        /// <param name="config">The global <see cref="HttpConfiguration"/>.</param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
        }
    }
}