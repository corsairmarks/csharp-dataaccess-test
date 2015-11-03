namespace DataAccessTest.Repository
{
    /// <summary>
    /// A repository that has read and write access to the specified <typeparamref name="TEntity"/>.
    /// Composed of <see cref="IGenericReadRepository{TEntity}"/> and <see cref="IGenericWriteRepository{TEntity}"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for this repository.</typeparam>
    public interface IGenericRepository<TEntity> : IGenericReadRepository<TEntity>, IGenericWriteRepository<TEntity> where TEntity : class
    {
    }
}