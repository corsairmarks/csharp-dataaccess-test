namespace DataAccessTest.Repository.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A generic repository for Entity Framework.
    /// </summary>
    /// <remarks>
    /// See the MSDN suggested implementation if IDisposable: http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
    /// </remarks>
    /// <typeparam name="TContext">The type of <see cref="System.Entity.Data.DbContext"/> for this generic repository.</typeparam>
    /// <typeparam name="TEntity">The type of entity (which must by a set in the <see cref="System.Entity.Data.DbContext"/>) for this generic repository.</typeparam>
    public class EntityFrameworkGenericRepository<TContext, TEntity> : IDisposable, IGenericRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        #region Properties

        /// <summary>
        /// The database context.
        /// </summary>
        protected TContext Context { get; private set; }

        /// <summary>
        /// The <see cref="IDbSet{TEntity}"/> for the entity.
        /// </summary>
        protected IDbSet<TEntity> Set
        {
            get { return Context.Set<TEntity>(); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkGenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context"></param>
        public EntityFrameworkGenericRepository(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Context = context;
        }

        #endregion

        #region IGenericReadRepository<TEntity> Members

        public virtual IQueryable<TEntity> GetAll()
        {
            return Set;
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Set.Where(predicate);
        }

        #endregion

        #region IGenericWriteRepository<TEntity> Members

        public virtual void Create(TEntity entity)
        {
            Set.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
                Set.Attach(entity);
            Set.Remove(entity);
        }

        #endregion

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

        ~EntityFrameworkGenericRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}