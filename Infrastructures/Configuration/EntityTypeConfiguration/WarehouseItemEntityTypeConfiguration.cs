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
    public class WarehouseItemEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseItem>
    {
        public void Configure(EntityTypeBuilder<WarehouseItem> builder)
        {
            builder.ToTable("WarehouseItems");

            builder.HasKey(wi => wi.Id);

             
            builder.Property(wi => wi.WarehouseId)
                   .IsRequired();

            builder.Property(wi => wi.ProductId)
                   .IsRequired();

            builder.Property(wi => wi.ReorderLevel)
                   .IsRequired();

            builder.Property(wi => wi.Quantity)
                   .IsRequired();

            
            builder.HasOne(wi => wi.Warehouse)
                   .WithMany(w => w.Items)
                   .HasForeignKey(wi => wi.WarehouseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(wi => wi.Product)
                   .WithMany(a => a.StockByWarehouse) 
                   .HasForeignKey(wi => wi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);  

             
            builder.HasIndex(wi => new { wi.WarehouseId, wi.ProductId })
                   .IsUnique();
        }
    }
}
