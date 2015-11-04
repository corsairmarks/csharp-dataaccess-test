namespace DataAccessTest.Web.DependencyResolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http.Dependencies;
    using Microsoft.Practices.ServiceLocation;
    using StructureMap;

    /// <summary>
    /// The structure map dependency scope.
    /// </summary>
    public class StructureMapDependencyScope : ServiceLocatorImplBase, IDependencyScope
    {
        #region Fields

        /// <summary>
        /// The container.
        /// </summary>
        protected readonly IContainer Container;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyScope"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="container"/> is <c>null</c>.</exception>
        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            this.Container.Dispose();
        }

        #endregion

        #region IDependencyScope Members

        /// <summary>
        /// Get all services of type <paramref name="serviceType"/>.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Collection of service instance objects.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.GetAllInstances(serviceType).Cast<object>();
        }

        #endregion

        #region Overridden Members

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of
        /// resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>Collection of service instance objects.</returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return this.Container.GetAllInstances(serviceType).Cast<object>();
        }

        /// <summary>
        /// When implemented by inheriting classes, this method will do the actual work of resolving
        /// the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>The requested service instance.</returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            object instance;
            var isAbstractOrInterface = serviceType.IsAbstract || serviceType.IsInterface;
            var hasKey = string.IsNullOrWhiteSpace(key);
            if (hasKey)
            {
                instance = isAbstractOrInterface
                    ? this.Container.TryGetInstance(serviceType)
                    : this.Container.GetInstance(serviceType);
            }
            else
            {
                instance = isAbstractOrInterface
                    ? this.Container.TryGetInstance(serviceType, key)
                    : this.Container.GetInstance(serviceType, key);
            }

            return instance;
        }

        #endregion
    }
}