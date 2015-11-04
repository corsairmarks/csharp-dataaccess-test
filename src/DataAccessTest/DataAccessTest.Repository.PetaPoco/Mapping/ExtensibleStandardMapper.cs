namespace DataAccessTest.Repository.PetaPoco.Mapping
{
    using System;
    using System.Reflection;
    using global::PetaPoco;

    /// <summary>
    /// <see cref="ExtensibleStandardMapper"/> is a duplicate of the <see cref="StandardMapper"/>, the default implementation of <see cref="IMapper"/> used by PetaPoco, but with virtual methods.
    /// </summary>
    public class ExtensibleStandardMapper : IMapper
    {
        /// <summary>
        /// Constructs a <see cref="TableInfo"/> for a POCO by reading its attribute data.
        /// </summary>
        /// <param name="pocoType">The POCO <see cref="Type"/>.</param>
        /// <returns>A <see cref="TableInfo"/> metadata object describing the table for <paramref name="pocoType"/>.</returns>
        public virtual TableInfo GetTableInfo(Type pocoType)
        {
            return TableInfo.FromPoco(pocoType);
        }

        /// <summary>
        /// Constructs a <see cref="ColumnInfo"/> for a POCO property by reading its attribute data.
        /// </summary>
        /// <param name="pocoProperty">Information on the POCO <see cref="PropertyInfo"/> to map to a column.</param>
        /// <returns>A <see cref="ColumnInfo"/> metadata object describing the column for <paramref name="pocoProperty"/>.</returns>
        public virtual ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
        {
            return ColumnInfo.FromProperty(pocoProperty);
        }

        /// <summary>
        /// Get a function that can convert into <paramref name="targetProperty"/> from the <paramref name="sourceType"/>.
        /// </summary>
        /// <remarks>
        /// A return value of <c>null</c> use the default conversion for PetaPoco.
        /// </remarks>
        /// <param name="targetProperty">Metadata about the target property on a POCO into which the database data will be mapped.</param>
        /// <param name="sourceType">The <see cref="Type"/> of the source column in the database.</param>
        /// <returns>A function that converts from <paramref name="sourceType"/> into <paramref name="targetProperty"/>.</returns>
        public virtual Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            return null;
        }

        /// <summary>
        /// Get a function that can convert from <paramref name="sourceProperty"/> into the appropriate database type.
        /// </summary>
        /// <remarks>
        /// A return value of <c>null</c> use the default conversion for PetaPoco.
        /// </remarks>
        /// <param name="sourceProperty">Metadata about the source property on a POCO from which the data will be mapped.</param>
        /// <returns>A function that converts from <paramref name="sourceProperty"/> into the correct, implementation-defined database type.</returns>
        public virtual Func<object, object> GetToDbConverter(PropertyInfo sourceProperty)
        {
            return null;
        }
    }
}