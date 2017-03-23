using BC_SecurityModule.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace BC_SecurityModule.DataModel.Configurations
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("dbo.DimCustomer");

            HasKey(e => e.CustomerKey);

            Property(e => e.CustomerLabel)
                .IsRequired()
                .HasMaxLength(100);

            Property(e => e.Title)
                .HasMaxLength(8);

            Property(e => e.FirstName)
                .HasMaxLength(50);

            Property(e => e.MiddleName)
                .HasMaxLength(50);

            Property(e => e.LastName)
                .HasMaxLength(50);

            Property(e => e.BirthDate)
                .HasColumnType("date");

            Property(e => e.MaritalStatus)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.Suffix)
                .HasMaxLength(10);

            Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.EmailAddress)
                .HasMaxLength(50);

            Property(e => e.YearlyIncome)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            Property(e => e.Education)
                .HasMaxLength(40)
                .IsFixedLength();

            Property(e => e.Occupation)
                .HasMaxLength(100);

            Property(e => e.HouseOwnerFlag)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.AddressLine1)
                .HasMaxLength(120);

            Property(e => e.AddressLine2)
                .HasMaxLength(120);

            Property(e => e.Phone)
                .HasMaxLength(20);

            Property(e => e.DateFirstPurchase)
                .HasColumnType("date");

            Property(e => e.CustomerType)
                .HasMaxLength(15);

            Property(e => e.CompanyName)
                .HasMaxLength(100);
        }

    }
}
