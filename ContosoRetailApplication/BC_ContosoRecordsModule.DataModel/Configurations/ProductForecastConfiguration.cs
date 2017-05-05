using BC_ContosoRecordsModule.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BC_ContosoRecordsModule.DataModel.Configurations
{
    public class ProductForecastConfiguration : EntityTypeConfiguration<ProductForecast>
    {
        public ProductForecastConfiguration()
        {
            ToTable("dbo.V_ProductForecast");

            HasKey(e => new {
                e.CalendarMonth,
                e.ProductCategoryName
            });

            Property(e => e.CalendarMonth)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.ProductCategoryName)
                .HasColumnOrder(1)
                .HasMaxLength(30);

            Property(e => e.SalesAmount)
                .HasPrecision(19, 4)
                .HasColumnType("money");
        }
    }
}
