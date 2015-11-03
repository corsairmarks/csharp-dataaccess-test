using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessTest.Library.Value;

namespace DataAccessTest.Repository.EntityFramework.Mapping
{
    internal class ValueModelMapping : EntityTypeConfiguration<ValueModel>
    {
        public ValueModelMapping()
        {
            ToTable("Value", "dbo");
            HasKey(r => r.Id)
                .Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.Description)
                .HasColumnName("Desc")
                .IsRequired()
                .HasMaxLength(256);
        }
    }
}
