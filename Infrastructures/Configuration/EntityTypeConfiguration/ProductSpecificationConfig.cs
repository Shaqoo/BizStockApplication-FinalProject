using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class ProductSpecificationConfig : IEntityTypeConfiguration<ProductSpecification>
    {
        public void Configure(EntityTypeBuilder<ProductSpecification> builder)
        {
            builder.ToTable("ProductSpecifications");

            builder.HasKey(ps => ps.Id);

            builder.Property(ps => ps.Value)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(ps => ps.Product)
                   .WithMany(p => p.ProductSpecifications)
                   .HasForeignKey(ps => ps.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(ps => ps.Specification)
            //       .WithMany(s => s.ProductSpecifications)
            //       .HasForeignKey(ps => ps.SpecificationId)
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
