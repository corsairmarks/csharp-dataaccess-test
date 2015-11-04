namespace DataAccessTest.Web.DependencyResolution.Registry
{
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    /// <summary>
    /// Configures dependency resolution for connecting to a SQL database.
    /// </summary>
    public class SqlDatabaseRegistry : Registry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDatabaseRegistry"/> class.
        /// </summary>
        public SqlDatabaseRegistry()
        {
            this.ForSingletonOf<string>().Use<string>("DefaultConnection").Named("DefaultConnectionStringName");
            this.ForSingletonOf<string>().Use(c => ConfigurationManager.ConnectionStrings[c.GetInstance<string>("DefaultConnectionStringName")].ConnectionString).Named("DefaultConnectionString");
            this.ForSingletonOf<IDbConnectionFactory>().Use<SqlConnectionFactory>().Ctor<string>().Named("DefaultConnectionString");
            this.For<DbConnection>()
                .LifecycleIs<ContainerLifecycle>()
                .Use(c => c.GetInstance<IDbConnectionFactory>().CreateConnection(c.GetInstance<string>("DefaultConnectionString")));
            this.Forward<DbConnection, IDbConnection>();
            this.ForSingletonOf<string>().Use<string>("System.Data.SqlClient").Named("DbProviderInvariantName");
            this.ForSingletonOf<DbProviderFactory>().Use(SqlClientFactory.Instance);
        }
    }
}