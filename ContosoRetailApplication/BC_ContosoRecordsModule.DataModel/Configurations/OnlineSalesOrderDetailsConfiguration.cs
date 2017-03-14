
using BC_ContosoRecordsModule.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace BC_ContosoRecordsModule.DataModel.Configurations
{
    public class OnlineSalesOrderDetailsConfiguration : EntityTypeConfiguration<OnlineSalesOrderDetail>
    {
        public OnlineSalesOrderDetailsConfiguration()
        {
            ToTable("dbo.V_OnlineSalesOrderDetails");

            HasKey(e => e.OrderNumber);

            Property(e => e.OrderNumber)
                .HasMaxLength(20);

            Property(e => e.Product)
                .HasMaxLength(500);
        }
    }
}
