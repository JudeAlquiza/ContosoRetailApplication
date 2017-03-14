
using BC_ContosoRecordsModule.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BC_ContosoRecordsModule.DataModel.Configurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("dbo.V_Customer");

            HasKey(e => e.CustomerKey);

            Property(e => e.CustomerKey)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.MaritalStatus)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.YearlyIncome)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            Property(e => e.Education)
                .HasMaxLength(40)
                .IsFixedLength();

            Property(e => e.HouseOwnerFlag)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.Consumption)
                .HasPrecision(19, 4)
                .HasColumnType("money");
        }

    }
}
