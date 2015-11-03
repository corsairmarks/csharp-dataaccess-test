namespace DataAccessTest.Web
{
    using System.Web.Mvc;

    /// <summary>
    /// MVC filter configuration for the application.
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Configure the global MVC filters for the application.
        /// </summary>
        /// <param name="filters">The global <see cref="GlobalFilterCollection"/>.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}