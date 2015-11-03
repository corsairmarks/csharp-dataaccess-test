namespace DataAccessTest.Repository.EntityFramework.Sample
{
    using System.Data.Common;
    using System.Data.Entity;
    using System.Reflection;
    using DataAccessTest.Library.Value;

    /// <summary>
    /// A sample <see cref="DbContext"/> for this proof-of-concept application.
    /// </summary>
    public class SampleDbContext : DbContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDbContext"/> class.
        /// </summary>
        /// <param name="conn">An open <see cref="DbConnection"/>.</param>
        public SampleDbContext(DbConnection conn)
            : base(conn, false)
        {
        }

        #endregion

        public IDbSet<ValueModel> Values { get; set; }

        #region Overridden Members

        /// <summary>
        /// This method is called when the model for a derived context has been initialized,
        /// but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // EF has ON CASCADE DELETE set to ON by default for foreign key relationships specified in the POCOs.
            // See: http://locktar.wordpress.com/2011/05/12/default-cascading-delete-in-ef-code-first/
            // Complex foreign keys create multiple paths for the CASCADE, which SQL Server refuses.
            // This means when deleting records, it will be necessary to delete their "child records" manually first.
            // Code is left here in case it is needed.
            // modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // TODO: see if I can find a better way with IoC - but this works for now
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }

        #endregion
    }
}