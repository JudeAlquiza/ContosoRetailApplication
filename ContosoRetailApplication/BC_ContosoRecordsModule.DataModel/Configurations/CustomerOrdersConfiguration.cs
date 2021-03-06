﻿
using BC_ContosoRecordsModule.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace BC_ContosoRecordsModule.DataModel.Configurations
{
    public class CustomerOrdersConfiguration : EntityTypeConfiguration<CustomerOrders>
    {
        public CustomerOrdersConfiguration()
        {
            ToTable("dbo.CustomerOrdersTop300K");
            //ToTable("dbo.V_CustomerOrders");

            HasKey(e => new {
                e.ProductCategoryName,
                e.ProductSubcategory,
                e.CustomerKey,
                e.IncomeGroup,
                e.CalendarYear,
                e.FiscalYear,
                e.Month,
                e.OrderNumber,
                e.Quantity,
                e.Amount
            });

            Property(e => e.ProductCategoryName)
                .HasColumnOrder(0)
                .HasMaxLength(30);

            Property(e => e.ProductSubcategory)
                .HasColumnOrder(1)
                .HasMaxLength(50);

            Property(e => e.Product)
                .HasMaxLength(500);

            Property(e => e.CustomerKey)
                .HasColumnOrder(2)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Region)
                .HasMaxLength(100);            

            Property(e => e.IncomeGroup)
                .HasColumnOrder(3)
                .HasMaxLength(8)
                .IsUnicode(false);

            Property(e => e.CalendarYear)
                .HasColumnOrder(4)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.FiscalYear)
                .HasColumnOrder(5)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Month)
                .HasColumnOrder(6)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.OrderNumber)
                .HasColumnOrder(7)
                .HasMaxLength(20);

            Property(e => e.Quantity)
                .HasColumnOrder(8)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Amount)
                .HasPrecision(19, 4)
                .HasColumnOrder(9)
                .HasColumnType("money");
        }
    }
}
