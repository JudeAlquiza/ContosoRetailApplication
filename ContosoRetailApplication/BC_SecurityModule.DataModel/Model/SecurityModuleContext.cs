using BC_SecurityModule.Core.Entities;
using BC_SecurityModule.DataModel.Configurations;
using System.Data.Entity;

namespace BC_SecurityModule.DataModel
{
    public class SecurityModuleContext : DbContext
    {
        public SecurityModuleContext()
            : base("name=SecurityConnectionString")
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new EmployeeConfiguration());
        }
    }
}
