namespace DataAccessTest.Repository.EntityFramework
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// An Entity Framework implementation of the Unit of Work pattern.
    /// </summary>
    /// <remarks>
    /// See the MSDN suggested implementation if IDisposable: http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
    /// </remarks>
    /// <typeparam name="TContext">The type of <see cref="System.Data.Entity.DbContext"/> for this unit of work.</typeparam>
    public class EntityFrameworkUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        #region Constructors

        public EntityFrameworkUnitOfWork(TContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            Context = context;
        }

        #endregion

        #region Properties

        protected TContext Context { get; private set; }

        #endregion

        #region IUnitOfWorkMembers

        public void Complete()
        {
            Context.SaveChanges();
        }

        #region IDisposable Members

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // free managed resources
                    if (Context != null)
                    {
                        Context.Dispose();
                        Context = null;
                    }
                }
                // free native resources if there are any.
                //if (nativeResource != IntPtr.Zero)
                //{
                //	Marshal.FreeHGlobal(nativeResource);
                //	nativeResource = IntPtr.Zero;
                //}

                disposed = true;
            }
        }

        ~EntityFrameworkUnitOfWork()
        {
            Dispose(false);
        }

        #endregion

        #endregion
    }
}