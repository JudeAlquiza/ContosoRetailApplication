
using BC_ContosoRecordsModule.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BC_ContosoRecordsModule.DataModel.Configurations
{
    public class CustomerPromotionConfiguration : EntityTypeConfiguration<CustomerPromotion>
    {
        public CustomerPromotionConfiguration()
        {
            ToTable("dbo.V_CustomerPromotion");

            HasKey(e => e.CustomerKey);
            HasKey(e => e.ProductKey);
            HasKey(e => e.PromotionKey);

            Property(e => e.CustomerKey)
                .HasColumnOrder(0)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.MaritalStatus)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.Gender)
                .HasMaxLength(1);

            Property(e => e.YearlyIncome)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            Property(e => e.Education)
                .HasMaxLength(40);

            Property(e => e.HouseOwnerFlag)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.PromotionName)
                .HasMaxLength(100);

            Property(e => e.PromotionType)
                .HasMaxLength(50);

            Property(e => e.ProductKey)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.PromotionKey)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
