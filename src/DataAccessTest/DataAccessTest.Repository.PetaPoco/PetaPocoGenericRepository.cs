namespace DataAccessTest.Repository.PetaPoco
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using DataAccessTest.Repository;
    using global::PetaPoco;

    /// <summary>
    /// A generic repository for PetaPoco.
    /// </summary>
    /// <remarks>
    /// See the MSDN suggested implementation if <see cref="IDisposable"/>: <c>http://msdn.microsoft.com/en-us/library/system.idisposable.aspx</c>
    /// </remarks>
    /// <typeparam name="TEntity">The type of entity for this generic repository.</typeparam>
    public class PetaPocoGenericRepository<TEntity> : IGenericRepository<TEntity>
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
        /// Initializes a new instance of the <see cref="PetaPocoGenericRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="database">The PetaPoco database.</param>
        public PetaPocoGenericRepository(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            this.Database = database;
        }

        #endregion

        #region Desructors

        /// <summary>
        /// Finalizes an instance of the <see cref="PetaPocoGenericRepository{TEntity}"/> class.
        /// </summary>
        ~PetaPocoGenericRepository()
        {
            this.Dispose(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the PetaPoco database.
        /// </summary>
        protected Database Database { get; private set; }

        #endregion

        #region IGenericReadRepository<TEntity> Members

        /// <summary>
        /// Get all of the entities.
        /// </summary>
        /// <returns>A query-able collection of entities.</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return this.Database.Query<TEntity>(string.Empty).AsQueryable();
        }

        /// <summary>
        /// Get all entities matching the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The entity filter.</param>
        /// <returns>The filtered entities.</returns>
        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            // DANGER: This will select the whole table every time - real code should consider writing specialized query methods when using PetaPoco
            return this.GetAll().Where(predicate);
        }

        #endregion

        #region IGenericWriteRepository<TEntity> Members

        /// <summary>
        /// Create the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public virtual void Create(TEntity entity)
        {
            this.Database.Insert(entity);
        }

        /// <summary>
        /// Update the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual void Update(TEntity entity)
        {
            this.Database.Update(entity);
        }

        /// <summary>
        /// Delete the entity form the data store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(TEntity entity)
        {
            this.Database.Delete(entity);
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
                    if (this.Database != null)
                    {
                        this.Database.Dispose();
                        this.Database = null;
                    }
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}