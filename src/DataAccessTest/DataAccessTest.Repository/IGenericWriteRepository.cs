namespace DataAccessTest.Repository
{
    /// <summary>
    /// A repository that has write-only access to the specified <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for this repository.</typeparam>
    public interface IGenericWriteRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Create the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        void Create(TEntity entity);

        /// <summary>
        /// Update the <paramref name="entity"/> in the data store.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete the entity form the data store.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(TEntity entity);
    }
}