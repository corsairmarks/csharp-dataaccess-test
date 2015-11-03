namespace DataAccessTest.Repository.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    /// <summary>
    /// Perform lifetime setup for Entity Framework data access.
    /// </summary>
    public class EntityFrameworkDbInitializer : IDataAccessInitializer
    {
        #region Variables

        /// <summary>
        /// A function to get the Entity Framework <see cref="DbConfiguration"/>.
        /// </summary>
        private readonly Func<DbConfiguration> getDbConfiguration;

        /// <summary>
        /// A function to retrieve all the Entity Framework contexts to initialize.
        /// </summary>
        private readonly Func<IEnumerable<DbContext>> getDbContexts;

        /// <summary>
        /// Whether the repositories have been initialized.
        /// </summary>
        private bool isInitialized;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkDbInitializer"/> class.
        /// </summary>
        /// <param name="getDbConfiguration">A function to get the Entity Framework <see cref="DbConfiguration"/>.</param>
        /// <param name="getDbContexts">A function to retrieve all the Entity Framework contexts to initialize.</param>
        public EntityFrameworkDbInitializer(Func<DbConfiguration> getDbConfiguration, Func<IEnumerable<DbContext>> getDbContexts)
        {
            if (getDbConfiguration == null)
            {
                throw new ArgumentNullException("getDbConfiguration");
            }

            if (getDbContexts == null)
            {
                throw new ArgumentNullException("getDbContexts");
            }

            this.getDbConfiguration = getDbConfiguration;
            this.getDbContexts = getDbContexts;
        }

        #endregion

        #region IRepositoryInitializer Members

        /// <summary>
        /// Perform any initialization required for the repositories.  This should be called
        /// once when the application domain is being initialized.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when this method is called and the repositories have already been initialized.
        /// </exception>
        public void Initialize()
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("Initialize() can only be called once per application.");
            }

            DbConfiguration.SetConfiguration(this.getDbConfiguration());

            foreach (var context in this.getDbContexts())
            {
                context.Database.Initialize(false);
                context.Dispose();
            }

            this.isInitialized = true;
        }

        #endregion
    }
}