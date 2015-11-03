namespace DataAccessTest.Web
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// MVC route configuration for the application.
    /// </summary>
    public static class RouteConfig
    {
        /// <summary>
        /// Configure the MVC routes for the application.
        /// </summary>
        /// <param name="routes">The global <see cref="RouteCollection"/>.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Help", action = "Index", id = UrlParameter.Optional });
        }
    }
}
