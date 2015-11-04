namespace DataAccessTest.Repository.EntityFramework
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// An Entity Framework implementation of the Unit of Work pattern.
    /// </summary>
    /// <remarks>
    /// See the MSDN suggested implementation if <see cref="IDisposable"/>: <c>http://msdn.microsoft.com/en-us/library/system.idisposable.aspx</c>
    /// </remarks>
    /// <typeparam name="TContext">The type of <see cref="DbContext"/> for this unit of work.</typeparam>
    public class EntityFrameworkUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        #region Fields

        /// <summary>
        /// Whether this instance has been disposed.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkUnitOfWork{TContext}"/> class.
        /// </summary>
        /// <param name="context">The Entity Framework context.</param>
        public EntityFrameworkUnitOfWork(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.Context = context;
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Finalizes an instance of the <see cref="EntityFrameworkUnitOfWork{TContext}"/> class.
        /// </summary>
        ~EntityFrameworkUnitOfWork()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Entity Framework context.
        /// </summary>
        protected TContext Context { get; private set; }

        #endregion

        #region IUnitOfWorkMembers

        /// <summary>
        /// Mark the unit of work as complete, which commits all modifications to the data store using Entity Framework.
        /// </summary>
        public void Complete()
        {
            this.Context.SaveChanges();
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
                    if (this.Context != null)
                    {
                        this.Context.Dispose();
                        this.Context = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion

        #endregion
    }
}