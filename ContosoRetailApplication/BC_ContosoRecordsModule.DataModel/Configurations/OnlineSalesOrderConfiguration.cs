
using BC_ContosoRecordsModule.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BC_ContosoRecordsModule.DataModel.Configurations
{
    public class OnlineSalesOrderConfiguration : EntityTypeConfiguration<OnlineSalesOrder>
    {
        public OnlineSalesOrderConfiguration()
        {
            ToTable("dbo.V_OnlineSalesOrder");

            HasKey(e => e.OrderNumber);
            HasKey(e => e.CustomerKey);
            HasKey(e => e.IncomeGroup);

            Property(e => e.OrderNumber)
                .HasColumnOrder(0)
                .HasMaxLength(20);

            Property(e => e.CustomerKey)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Region)
                .HasColumnOrder(100);

            Property(e => e.IncomeGroup)
                .HasColumnOrder(2)
                .HasMaxLength(8)
                .IsUnicode(false);
        }
    }
}
