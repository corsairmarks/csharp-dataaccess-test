namespace DataAccessTest.Repository.NHibernate
{
    using System;
    using System.Collections.Generic;
    using global::NHibernate.Cfg;
    using global::NHibernate.Dialect;
    using global::NHibernate.Mapping.ByCode;

    /// <summary>
    /// Perform lifetime setup for NHibernate data access.
    /// </summary>
    public class NHibernateDbInitializer : IDataAccessInitializer
    {
        #region Fields

        /// <summary>
        /// The NHibernate configuration.
        /// </summary>
        private readonly Configuration configuration;

        /// <summary>
        /// The database connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// A function to retrieve all the NHibernate contexts to initialize.
        /// </summary>
        private readonly Func<IEnumerable<IConformistHoldersProvider>> getClassMappings;

        /// <summary>
        /// Whether the data access layer has been initialized.
        /// </summary>
        private bool isInitialized;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateDbInitializer"/> class.
        /// </summary>
        /// <param name="configuration">The NHibernate <see cref="Configuration"/> to initialize.</param>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="getClassMappings">A function to retrieve all the NHibernate class mappings (which implement <see cref="IConformistHoldersProvider"/>) to initialize.</param>
        public NHibernateDbInitializer(Configuration configuration, string connectionString, Func<IEnumerable<IConformistHoldersProvider>> getClassMappings)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("dbConnectionString");
            }

            if (getClassMappings == null)
            {
                throw new ArgumentNullException("getDbContexts");
            }

            this.configuration = configuration;
            this.connectionString = connectionString;
            this.getClassMappings = getClassMappings;
        }

        #endregion

        #region IDataAccessInitializer Members

        /// <summary>
        /// Perform any initialization required for the data access layer.  This should be called
        /// once when the application domain is being initialized.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when this method is called and the data access layer has already been initialized.
        /// </exception>
        public void Initialize()
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("Initialize() can only be called once per application.");
            }

            this.configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = this.connectionString;
                db.Dialect<MsSql2012Dialect>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            });

            var mapper = new ModelMapper();
            foreach (var classMapping in this.getClassMappings())
            {
                mapper.AddMapping(classMapping);
            }

            this.configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            this.isInitialized = true;
        }

        #endregion
    }
}