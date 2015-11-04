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
    /// See the MSDN suggested implementation if <see cref="IDisposable"/>: <c>http://msdn.microsoft.com/en-us/library/system.idisposable.aspx</c>
    /// </remarks>
    /// <typeparam name="TContext">The type of <see cref="DbContext"/> for this generic repository.</typeparam>
    /// <typeparam name="TEntity">The type of entity (which must by a set in the <see cref="DbContext"/>) for this generic repository.</typeparam>
    public class EntityFrameworkGenericRepository<TContext, TEntity> : IDisposable, IGenericRepository<TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        #region Fields

        /// <summary>
        /// Whether this instance has been disposed.
        /// </summary>
        private bool disposed = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkGenericRepository{TContext, TEntity}"/> class.
        /// </summary>
        /// <param name="context">The Entity Framework context.</param>
        public EntityFrameworkGenericRepository(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.Context = context;
        }

        #endregion

        #region Desructors

        /// <summary>
        /// Finalizes an instance of the <see cref="EntityFrameworkGenericRepository{TContext, TEntity}"/> class.
        /// </summary>
        ~EntityFrameworkGenericRepository()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Entity Framework context.
        /// </summary>
        protected TContext Context { get; private set; }

        /// <summary>
        /// Gets the <see cref="IDbSet{TEntity}"/> for the entity.
        /// </summary>
        protected IDbSet<TEntity> Set
        {
            get { return this.Context.Set<TEntity>(); }
        }

        #endregion

        #region IGenericReadRepository<TEntity> Members

        /// <summary>
        /// Get all of the entities.
        /// </summary>
        /// <returns>A query-able collection of entities.</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return this.Set;
        }

        /// <summary>
        /// Get all entities matching the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The entity filter.</param>
        /// <returns>The filtered entities.</returns>
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Set.Where(predicate);
        }

        #endregion

        #region IGenericWriteRepository<TEntity> Members

        /// <summary>
        /// Create the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public virtual void Create(TEntity entity)
        {
            this.Set.Add(entity);
        }

        /// <summary>
        /// Update the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual void Update(TEntity entity)
        {
            this.Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete the entity form the data store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(TEntity entity)
        {
            if (this.Context.Entry(entity).State == EntityState.Detached)
            {
                this.Set.Attach(entity);
            }

            this.Set.Remove(entity);
        }

        #endregion

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
    }
}