using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class SpecificationConfig : IEntityTypeConfiguration<Specification>
    {
        public void Configure(EntityTypeBuilder<Specification> builder)
        {
            builder.ToTable("Specifications");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(s => s.Description)
                   .HasMaxLength(3000);

            //builder.HasMany<ProductSpecification>("_productSpecifications")
            //       .WithOne(ps => ps.Specification)
            //       .HasForeignKey(ps => ps.SpecificationId)
            //       .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
