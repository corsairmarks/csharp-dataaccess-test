namespace DataAccessTest.Web.DependencyResolution
{
    using System;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;
    using StructureMap;

    /// <summary>
    /// This <see cref="IHttpControllerActivator"/> enables StructureMap dependency injection into WebAPI
    /// controllers.  StructureMap does not work with WebAPI controllers out of the box
    /// because Microsoft uses a different facility for constructing and adding dependency
    /// injection than for MVC controllers.
    /// </summary>
    /// <remarks>
    /// Code taken from: <c>http://marisks.net/2013/01/22/better-way-to-configure-structuremap-in-aspnet-webapi/</c>
    /// Reasons for use over <see cref="IDependencyResolver"/>: <c>http://blog.ploeh.dk/2012/09/27/DependencyInjectionandLifetimeManagementwithASP.NETWebAPI/</c>
    /// </remarks>
    public class StructureMapHttpControllerActivator : IHttpControllerActivator
    {
        #region Fields

        /// <summary>
        /// The inversion-of-control container.
        /// </summary>
        private readonly IContainer container;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapHttpControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The inversion-of-control container instance from which to create a child container for object resolution.</param>
        public StructureMapHttpControllerActivator(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        #endregion

        #region IHttpControllerActivator Members

        /// <summary>
        /// Creates an <see cref="IHttpController"/> object.
        /// </summary>
        /// <param name="request">The message request.</param>
        /// <param name="controllerDescriptor">The HTTP controller descriptor.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>An <see cref="IHttpController"/> object.</returns>
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            // Using a nested container will automagically dispose any IDisposable types when it is disposed.
            // Nested containers: http://codebetter.com/jeremymiller/2010/02/10/nested-containers-in-structuremap-2-6-1/
            // This activator class: http://marisks.net/2013/01/26/disposables-structuremap-and-web-api-composition-root/
            var nestedContainer = this.container.GetNestedContainer();
            request.RegisterForDispose(nestedContainer);
            return nestedContainer.GetInstance(controllerType) as IHttpController;
        }

        #endregion
    }
}