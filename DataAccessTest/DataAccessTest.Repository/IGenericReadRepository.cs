namespace DataAccessTest.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// A repository that has read-only access to the specified <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for this repository.</typeparam>
    public interface IGenericReadRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all of the entities.
        /// </summary>
        /// <returns>A query-able collection of entities.</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Get all entities matching the <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The entity filter.</param>
        /// <returns>The filtered entities.</returns>
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
    }
}