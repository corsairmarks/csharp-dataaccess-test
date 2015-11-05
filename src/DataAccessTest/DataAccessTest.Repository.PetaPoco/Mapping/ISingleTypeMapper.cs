namespace DataAccessTest.Repository.PetaPoco.Mapping
{
    using System;
    using global::PetaPoco;

    /// <summary>
    /// A specialized <see cref="IMapper"/> designed to return information about only one <see cref="Type"/>.
    /// </summary>
    public interface ISingleTypeMapper : IMapper
    {
        /// <summary>
        /// Gets the <see cref="Type"/> mapped by this <see cref="IMapper"/>.
        /// </summary>
        Type MappedType { get; }
    }
}