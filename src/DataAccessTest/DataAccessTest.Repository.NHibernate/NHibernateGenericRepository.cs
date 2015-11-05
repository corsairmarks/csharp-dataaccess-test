namespace DataAccessTest.Repository.NHibernate
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using global::NHibernate;
    using global::NHibernate.Linq;

    /// <summary>
    /// A generic repository for NHibernate.
    /// </summary>
    /// <remarks>
    /// See the MSDN suggested implementation if <see cref="IDisposable"/>: <c>http://msdn.microsoft.com/en-us/library/system.idisposable.aspx</c>
    /// </remarks>
    /// <typeparam name="TEntity">The type of entity for this generic repository.</typeparam>
    public class NHibernateGenericRepository<TEntity> : IDisposable, IGenericRepository<TEntity>
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
        /// Initializes a new instance of the <see cref="NHibernateGenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="session">The NHibernate session.</param>
        public NHibernateGenericRepository(ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            this.Session = session;
        }

        #endregion

        #region Desructors

        /// <summary>
        /// Finalizes an instance of the <see cref="NHibernateGenericRepository{TEntity}"/> class.
        /// </summary>
        ~NHibernateGenericRepository()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the NHibernate session.
        /// </summary>
        protected ISession Session { get; private set; }

        #endregion

        #region IGenericReadRepository<TEntity> Members

        /// <summary>
        /// Get all of the entities.
        /// </summary>
        /// <returns>A query-able collection of entities.</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return this.Session.Query<TEntity>();
        }

        /// <summary>
        /// Get all entities matching the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The entity filter.</param>
        /// <returns>The filtered entities.</returns>
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Session.Query<TEntity>().Where(predicate);
        }

        #endregion

        #region IGenericWriteRepository<TEntity> Members

        /// <summary>
        /// Create the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public virtual void Create(TEntity entity)
        {
            this.Session.Save(entity);
        }

        /// <summary>
        /// Update the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual void Update(TEntity entity)
        {
            this.Session.Update(entity);
        }

        /// <summary>
        /// Delete the entity form the data store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(TEntity entity)
        {
            this.Session.Delete(entity);
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
                    if (this.Session != null)
                    {
                        this.Session.Dispose();
                        this.Session = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
