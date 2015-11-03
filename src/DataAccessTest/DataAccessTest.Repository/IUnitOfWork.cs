namespace DataAccessTest.Repository
{
    using System;

    /// <summary>
    /// A discrete unit of work that automatically rolls back all execution unless <see cref="Complete"/> is called.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Mark the unit of work as complete, which commits all modifications to the data store.
        /// </summary>
        void Complete();
    }
}