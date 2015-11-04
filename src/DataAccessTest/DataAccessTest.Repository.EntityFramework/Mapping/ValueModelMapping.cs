namespace DataAccessTest.Repository.EntityFramework.Mapping
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using DataAccessTest.Library.Value;

    /// <summary>
    /// Entity Framework field mapping for the <see cref="ValueModelMapping"/> class.
    /// </summary>
    internal class ValueModelMapping : EntityTypeConfiguration<ValueModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueModelMapping"/> class.
        /// </summary>
        public ValueModelMapping()
        {
            this.ToTable("Value", "dbo");
            this.HasKey(r => r.Id)
                .Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(r => r.Description)
                .HasColumnName("Desc")
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}