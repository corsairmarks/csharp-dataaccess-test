namespace DataAccessTest.Repository.NHibernate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using global::NHibernate;

    /// <summary>
    /// An NHibernate implementation of the Unit of Work pattern.
    /// </summary>
    /// <remarks>
    /// See the MSDN suggested implementation if <see cref="IDisposable"/>: <c>http://msdn.microsoft.com/en-us/library/system.idisposable.aspx</c>
    /// </remarks>
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        #region Fields

        /// <summary>
        /// Whether this instance has been disposed.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NHibernateUnitOfWork"/> class.
        /// </summary>
        /// <param name="transaction">The NHibernate transaction.</param>
        public NHibernateUnitOfWork(ITransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            this.Transaction = transaction;
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Finalizes an instance of the <see cref="NHibernateUnitOfWork"/> class.
        /// </summary>
        ~NHibernateUnitOfWork()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the NHibernate transaction.
        /// </summary>
        protected ITransaction Transaction { get; private set; }

        #endregion

        #region IUnitOfWorkMembers

        /// <summary>
        /// Mark the unit of work as complete, which commits all modifications to the data store using NHibernate.
        /// </summary>
        public void Complete()
        {
            this.Transaction.Commit();
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