using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class PurchaseOrderItemEntityTypeConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
            builder.ToTable("PurchaseOrderItems");

            builder.HasKey(poi => poi.Id);

            builder.Property(poi => poi.PurchaseOrderId)
                   .IsRequired();

            builder.Property(poi => poi.ProductId)
                   .IsRequired();

            builder.Property(poi => poi.ProductName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(poi => poi.QuantityOrdered)
                   .IsRequired();

            builder.Property(poi => poi.QuantityReceived)
                   .IsRequired();

            builder.Property(poi => poi.UnitPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            
            builder.Ignore(poi => poi.TotalPrice);
            builder.Ignore(poi => poi.IsFullyReceived);

            builder.HasOne(poi => poi.PurchaseOrder)
                   .WithMany(po => po.Items)
                   .HasForeignKey(poi => poi.PurchaseOrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(poi => poi.Product)
                   .WithMany() 
                   .HasForeignKey(poi => poi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(poi => new { poi.PurchaseOrderId, poi.ProductId });

        }
    }
}
