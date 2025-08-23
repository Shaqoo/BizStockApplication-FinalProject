using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating)
                   .IsRequired();

            builder.Property(r => r.Comment)
                   .HasMaxLength(1000)
                   .IsRequired();

            builder.Property(r => r.ReviewedAt)
                   .IsRequired();

            builder.Property(r => r.IsVisible)
                   .IsRequired();


          
            builder.HasOne(r => r.Reviewer)
                   .WithMany()
                   .HasForeignKey(r => r.ReviewerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Product)
                   .WithMany(p => p.Reviews)  
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(r => r.Supplier)
                   .WithMany(s => s.Reviews)
                   .HasForeignKey(r => r.SupplierId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(r => r.DeliveryAgent)
                   .WithMany(d => d.Reviews)
                   .HasForeignKey(r => r.DeliveryAgentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(r => r.IsVisible);
        }
    }
}
