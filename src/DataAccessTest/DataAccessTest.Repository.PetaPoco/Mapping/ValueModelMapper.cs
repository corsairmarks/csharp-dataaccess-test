namespace DataAccessTest.Repository.PetaPoco.Mapping
{
    using DataAccessTest.Library.Value;
    using global::PetaPoco;

    /// <summary>
    /// PetaPoco <see cref="IMapper"/> specific to the <see cref="ValueModel"/> class.
    /// </summary>
    public class ValueModelMapper : GenericExtensibleStandardMapper<ValueModel>
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
    }
}