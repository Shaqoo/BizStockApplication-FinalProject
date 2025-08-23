using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class PurchaseOrderEntityTypeConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder.ToTable("PurchaseOrders");

            builder.HasKey(po => po.Id);

            builder.Property(po => po.OrderNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(po => po.OrderNumber)
                   .IsUnique();

            builder.Property(po => po.SupplierId)
                   .IsRequired();

            builder.Property(po => po.ExpectedDeliveryDate);

            builder.Property(po => po.Status)
                   .HasConversion<string>() 
                   .IsRequired();

            builder.Property(po => po.SubTotal)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(po => po.Discount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(po => po.Tax)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Ignore(po => po.Total); 

            builder.Property(po => po.Notes)
                   .HasMaxLength(1000);

            builder.HasOne(po => po.Supplier)
                   .WithMany(s => s.PurchaseOrders) 
                   .HasForeignKey(po => po.SupplierId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(po => po.Items)
                   .WithOne(i => i.PurchaseOrder)
                   .HasForeignKey(i => i.PurchaseOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
