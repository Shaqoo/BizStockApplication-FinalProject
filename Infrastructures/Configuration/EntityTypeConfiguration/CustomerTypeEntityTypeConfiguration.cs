using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class CustomerTypeEntityTypeConfiguration : IEntityTypeConfiguration<CustomerType>
    {
        public void Configure(EntityTypeBuilder<CustomerType> builder)
        {
            builder.ToTable("CustomerTypes");

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.TypeName)
                   .HasConversion<string>() 
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(ct => ct.Description)
                   .HasMaxLength(500);

            builder.Property(ct => ct.DiscountPercentage)
                   .HasColumnType("decimal(5,2)");

            builder.Property(ct => ct.CreatedAt)
                   .IsRequired();

            builder.HasMany(ct => ct.Customers)
                   .WithOne(c => c.CustomerType)
                   .HasForeignKey(c => c.CustomerTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

           builder.HasData(
           new CustomerType(CustomerTypeName.Retail, "Retail customers", 0m),
           new CustomerType(CustomerTypeName.Wholesale, "Wholesale buyers with bulk discounts", 5m),
           new CustomerType(CustomerTypeName.Corporate, "Corporate clients with special contracts", 10m),
           new CustomerType(CustomerTypeName.Reseller, "Resellers who purchase for resale", 7.5m) ,
           new CustomerType(CustomerTypeName.VIP, "VIP customers with premium benefits", 15m) 
       );
        }
    }
}
