namespace DataAccessTest.Web.DependencyResolution
{
    using System.Web.Http.Dependencies;
    using StructureMap;

    /// <summary>
    /// The structure map dependency resolver.
    /// </summary>
    public class StructureMapDependencyResolver : StructureMapDependencyScope, IDependencyResolver
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StructureMapDependencyResolver(IContainer container)
            : base(container)
        {
        }

        #endregion

        #region IDependencyResolver Members

        /// <summary>
        /// Begins a new dependency scope as a child of this scope.
        /// </summary>
        /// <returns>The child <see cref="IDependencyScope"/>.</returns>
        public IDependencyScope BeginScope()
        {
            return new StructureMapDependencyResolver(this.Container.GetNestedContainer());
        }

        #endregion
    }
}