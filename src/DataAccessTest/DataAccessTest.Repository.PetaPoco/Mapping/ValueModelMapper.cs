namespace DataAccessTest.Repository.PetaPoco.Mapping
{
    using System.Reflection;
    using DataAccessTest.Library.Value;
    using DataAccessTest.Utility;
    using global::PetaPoco;

    /// <summary>
    /// PetaPoco <see cref="IMapper"/> specific to the <see cref="ValueModel"/> class.
    /// </summary>
    public class ValueModelMapper : GenericExtensibleSingleTypeMapper<ValueModel>
    {
        /// <summary>
        /// Constructs a <see cref="TableInfo"/> for the <see name="ValueModel"/> POCO.
        /// </summary>
        /// <returns>A <see cref="TableInfo"/> metadata object describing the table for <see cref="ValueModel"/>.</returns>
        protected override TableInfo GetTableInfoCore()
        {
            return new TableInfo
            {
                AutoIncrement = true,
                PrimaryKey = "Id",
                TableName = "Value",
            };
        }

        /// <summary>
        /// Constructs a <see cref="ColumnInfo"/> for a POCO property by reading its attribute data.
        /// </summary>
        /// <param name="pocoProperty">Information on the POCO <see cref="PropertyInfo"/> to map to a column.</param>
        /// <returns>A <see cref="ColumnInfo"/> metadata object describing the column for <paramref name="pocoProperty"/>.</returns>
        protected override ColumnInfo GetColumnInfoCore(PropertyInfo pocoProperty)
        {
            if (string.Equals(pocoProperty.Name, PropertyMapper<ValueModel>.PropertyName(vm => vm.Description)))
            {
                return new ColumnInfo
                {
                    ColumnName = "Desc",
                    ForceToUtc = false,
                    ResultColumn = false,
                };
            }

            return base.GetColumnInfoCore(pocoProperty);
        }
    }
}