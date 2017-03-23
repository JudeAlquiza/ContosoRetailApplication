using BC_SecurityModule.Core.Entities;
using System.Data.Entity.ModelConfiguration;

namespace BC_SecurityModule.DataModel.Configurations
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("dbo.DimEmployee");

            HasKey(e => e.EmployeeKey);

            HasMany(e => e.DimEmployee1)
               .WithOptional(e => e.DimEmployee2)
               .HasForeignKey(e => e.ParentEmployeeKey);

            Property(e => e.FirstName)
                 .IsRequired()
                 .HasMaxLength(50);

            Property(e => e.LastName)
                 .IsRequired()
                 .HasMaxLength(50);

            Property(e => e.MiddleName)
                .HasMaxLength(50);

            Property(e => e.Title)
                .HasMaxLength(50);

            Property(e => e.HireDate)
                .HasColumnType("date");

            Property(e => e.BirthDate)
                .HasColumnType("date");

            Property(e => e.EmailAddress)
                .HasMaxLength(50);

            Property(e => e.Phone)
                .HasMaxLength(50);

            Property(e => e.MaritalStatus)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.EmergencyContactName)
                .HasMaxLength(50);

            Property(e => e.EmergencyContactPhone)
                .HasMaxLength(25);

            Property(e => e.Gender)
                .HasMaxLength(1)
                .IsFixedLength();

            Property(e => e.BaseRate)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            Property(e => e.DepartmentName)
                .HasMaxLength(50);

            Property(e => e.StartDate)
                .HasColumnType("date");

            Property(e => e.EndDate)
                .HasColumnType("date");

            Property(e => e.Status)
                .HasMaxLength(50);
        }
    }
}
