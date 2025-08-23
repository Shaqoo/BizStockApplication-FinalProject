using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class SalesOrderEntityTypeConfiguration : IEntityTypeConfiguration<SalesOrder>
    {
        public void Configure(EntityTypeBuilder<SalesOrder> builder)
        {
            builder.ToTable("SalesOrders");

            builder.HasKey(so => so.Id);

            builder.Property(so => so.OrderNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(so => so.OrderNumber)
                   .IsUnique();

            builder.Property(so => so.CustomerId)
                   .IsRequired();

            builder.Property(so => so.ExpectedDeliveryDate);

            builder.Property(so => so.Status)
                   .HasConversion<string>() 
                   .IsRequired();

            builder.Property(so => so.SubTotal)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(so => so.Discount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(so => so.Tax)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Ignore(so => so.Total); 

            builder.Property(so => so.Note)
                   .HasMaxLength(1000);

            builder.HasOne(so => so.Customer)
                   .WithMany(c => c.SalesOrders)
                   .HasForeignKey(so => so.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(so => so.Items)
                   .WithOne(i => i.SalesOrder)
                   .HasForeignKey(i => i.SalesOrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
