namespace DataAccessTest.Repository.EntityFramework.Sample
{
    /// <summary>
    /// Seeds the sample database with predetermined data.
    /// </summary>
    public class SampleDbContextInitializer : SeedOnlyDatabaseInitializer<SampleDbContext>
    {
        /// <summary>
        /// Seeds initial data into the database.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(SampleDbContext context)
        {
            // Do nothing for now
        }
    }
}