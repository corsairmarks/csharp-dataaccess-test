namespace DataAccessTest.Repository.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Common;
    using System.Data.Entity.Infrastructure;
    using System.Linq;

    /// <summary>
    /// An instance of <see cref="DbConfiguration"/> that can be configured by inversion-of-control.
    /// </summary>
    /// <typeparam name="TContext">The type of <see cref="DbContext"/> to initialize.</typeparam>
    public class EntityFrameworkConfig<TContext> : DbConfiguration where TContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkConfig{TContext}"/> class.  This class sets up the database configuration for Entity Framework.
        /// </summary>
        /// <param name="dbConnectionFactory">The database connection factory.</param>
        /// <param name="providerInvariantName">The invariant provider name.</param>
        /// <param name="dbProviderFactory">The database provider factory.</param>
        /// <param name="dbProviderServices">The database provider services.</param>
        /// <param name="databaseInitializer">The database initializer.</param>
        public EntityFrameworkConfig(
            IDbConnectionFactory dbConnectionFactory,
            string providerInvariantName,
            DbProviderFactory dbProviderFactory,
            DbProviderServices dbProviderServices,
            IDatabaseInitializer<TContext> databaseInitializer)
            : base()
        {
            if (dbConnectionFactory == null)
            {
                throw new ArgumentNullException("dbConnectionFactory");
            }

            if (string.IsNullOrWhiteSpace(providerInvariantName))
            {
                throw new ArgumentNullException("providerInvariantName");
            }

            if (dbProviderFactory == null)
            {
                throw new ArgumentNullException("dbProviderFactory");
            }

            if (dbProviderServices == null)
            {
                throw new ArgumentNullException("dbProviderServices");
            }

            if (databaseInitializer == null)
            {
                throw new ArgumentNullException("databaseInitializer");
            }

            this.SetDefaultConnectionFactory(dbConnectionFactory);
            this.SetProviderFactory(providerInvariantName, dbProviderFactory);
            this.SetProviderServices(providerInvariantName, dbProviderServices);
            this.SetDatabaseInitializer(databaseInitializer);
        }

        #endregion
    }
}