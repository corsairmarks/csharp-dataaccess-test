namespace DataAccessTest.Repository.NHibernate.Mapping
{
    using DataAccessTest.Library.Value;
    using global::NHibernate.Mapping.ByCode;
    using global::NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// NHibernate database model mapping for the <see cref="ValueModel"/> class.
    /// </summary>
    public class ValueModelMapping : ClassMapping<ValueModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueModelMapping"/> class.
        /// </summary>
        public ValueModelMapping()
        {
            this.Table("Value");
            this.Schema("dbo");
            this.Id(
                vm => vm.Id,
                m =>
                {
                    m.Generator(Generators.Identity);
                });

            this.Property(
                vm => vm.Description,
                m =>
                {
                    m.Column("Desc");
                    m.Length(256);
                    m.NotNullable(true);
                });
        }
    }
}