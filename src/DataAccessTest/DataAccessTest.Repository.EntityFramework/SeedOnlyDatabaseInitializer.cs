namespace DataAccessTest.Repository.EntityFramework
{
    using System.Data.Entity;

    /// <summary>
    /// An abstract <see cref="IDatabaseInitializer"/> designed to seed data but not actually alter a database.
    /// </summary>
    /// <typeparam name="TContext">The type of <see cref="DbContext"/> to initialize.</typeparam>
    public abstract class SeedOnlyDatabaseInitializer<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        #region IDatabaseInitializer<TContext> Members

        /// <summary>
        /// Executes the strategy to initialize the database for the given context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void InitializeDatabase(TContext context)
        {
            // Don't try to alter the database, assume something else does that for us
            this.Seed(context);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Seeds initial data into the database.
        /// </summary>
        /// <param name="context">The context.</param>
        protected abstract void Seed(TContext context);

        #endregion
    }
}