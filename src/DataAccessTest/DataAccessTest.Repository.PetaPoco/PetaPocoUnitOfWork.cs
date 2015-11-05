namespace DataAccessTest.Repository.PetaPoco
{
    using System;
    using global::PetaPoco;

    /// <summary>
    /// A PetaPoco implementation of the Unit of Work pattern.
    /// </summary>
    /// <remarks>
    /// See the MSDN suggested implementation if <see cref="IDisposable"/>: <c>http://msdn.microsoft.com/en-us/library/system.idisposable.aspx</c>
    /// </remarks>
    public class PetaPocoUnitOfWork : IUnitOfWork
    {
        #region Fields

        /// <summary>
        /// Whether this instance has been disposed.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PetaPocoUnitOfWork"/> class.
        /// </summary>
        /// <param name="transaction">The PetaPoco transaction.</param>
        public PetaPocoUnitOfWork(ITransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("database");
            }

            this.Transaction = transaction;
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Finalizes an instance of the <see cref="PetaPocoUnitOfWork"/> class.
        /// </summary>
        ~PetaPocoUnitOfWork()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the PetaPoco transaction.
        /// </summary>
        protected ITransaction Transaction { get; private set; }

        #endregion

        #region IUnitOfWorkMembers

        /// <summary>
        /// Mark the unit of work as complete, which commits all modifications to the data store using Entity Framework.
        /// </summary>
        public void Complete()
        {
            this.Transaction.Complete();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (this.Transaction != null)
                    {
                        this.Transaction.Dispose();
                        this.Transaction = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion

        #endregion
    }
}