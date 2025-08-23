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
    public class SalesOrderItemEntityTypeConfiguration : IEntityTypeConfiguration<SalesOrderItem>
    {
        public void Configure(EntityTypeBuilder<SalesOrderItem> builder)
        {
            builder.ToTable("SalesOrderItems");

            builder.HasKey(soi => soi.Id);

            builder.Property(soi => soi.SalesOrderId)
                   .IsRequired();

            builder.Property(soi => soi.ProductId)
                   .IsRequired();

            builder.Property(soi => soi.ProductName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(soi => soi.Quantity)
                   .IsRequired();

            builder.Property(soi => soi.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Ignore(soi => soi.TotalPrice); 

            builder.HasOne(soi => soi.SalesOrder)
                   .WithMany(so => so.Items)
                   .HasForeignKey(soi => soi.SalesOrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(soi => soi.Product)
                   .WithMany() 
                   .HasForeignKey(soi => soi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(soi => new { soi.SalesOrderId, soi.ProductId });
        }
    }
}
