using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    namespace Infrastructure.Persistence.Configurations
    {
        public class BrandConfiguration : IEntityTypeConfiguration<Brand>
        {
            public void Configure(EntityTypeBuilder<Brand> builder)
            {
                builder.ToTable("Brands");

                builder.HasKey(b => b.Id);

                builder.Property(b => b.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(b => b.Description)
                    .HasMaxLength(500);

                builder.Property(b => b.WebsiteUrl)
                    .HasMaxLength(255);

                builder.Property(b => b.LogoUrl)
                    .HasMaxLength(255);

                builder.Property(b => b.IsActive)
                    .IsRequired();

                builder.Property(b => b.CreatedAt)
                    .IsRequired();

                builder.Property(b => b.UpdatedAt);

                builder.HasMany(b => b.Products)
                       .WithOne(p => p.Brand)
                       .HasForeignKey(p => p.BrandId)
                       .OnDelete(DeleteBehavior.Restrict); 
            }
        }
    }

}
