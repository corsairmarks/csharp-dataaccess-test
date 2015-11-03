namespace DataAccessTest.Repository
{
    /// <summary>
    /// Implement in order to perform lifetime setup for a data access technology.
    /// </summary>
    public interface IDataAccessInitializer
    {
        /// <summary>
        /// Perform any initialization required for the data access technology.  This should be called
        /// once when the application domain is being initialized.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when this method is called and the data access technology has already been initialized.
        /// </exception>
        void Initialize();
    }
}