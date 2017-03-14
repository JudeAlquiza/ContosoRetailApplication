using BC_ContosoRecordsModule.Core.Entities;
using BC_ContosoRecordsModule.DataModel.Configurations;
using System.Data.Entity;

namespace BC_ContosoRecordsModule.DataModel.Model
{
    public class ContosoRecordsModuleContext : DbContext
    {
        public ContosoRecordsModuleContext()
            : base("name=ContosoRecordsConnectionString")
        {
        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerOrders> CustomerOrders { get; set; }
        public virtual DbSet<CustomerPromotion> CustomerPromotion { get; set; }
        public virtual DbSet<OnlineSalesOrder> OnlineSalesOrder { get; set; }
        public virtual DbSet<OnlineSalesOrderDetail> OnlineSalesOrderDetail { get; set; }
        public virtual DbSet<ProductForecast> ProductForecast { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new CustomerOrdersConfiguration());
            modelBuilder.Configurations.Add(new CustomerPromotionConfiguration());
            modelBuilder.Configurations.Add(new OnlineSalesOrderConfiguration());
            modelBuilder.Configurations.Add(new OnlineSalesOrderDetailsConfiguration());
            modelBuilder.Configurations.Add(new ProductForecastConfiguration());
        }
    }
}
