using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(p => p.SKU)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.Barcode)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.QrCodeValue)
                   .HasMaxLength(250);

            builder.Property(p => p.Description)
                   .HasMaxLength(3000);

            builder.Property(p => p.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.CostPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(p => p.SellingPrice)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

         
            builder.Property(p => p.SearchVector)
            .HasColumnType("tsvector")
            .HasComputedColumnSql(
                "to_tsvector('english', coalesce(\"Name\", '') || ' ' || coalesce(\"Description\", ''))",
                stored: true
            );

            builder.HasIndex(p => p.SearchVector)
                .HasMethod("GIN");

            builder.Property(p => p.UnitOfMeasure)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(p => p.IsActive)
                   .IsRequired();

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Reviews)
                   .WithOne(r => r.Product)
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.StockByWarehouse)
                   .WithOne(wi => wi.Product)
                   .HasForeignKey(wi => wi.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProductTags)
                   .WithOne(pt => pt.Product)
                   .HasForeignKey(pt => pt.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ProductSpecifications)
                   .WithOne(p => p.Product)
                   .HasForeignKey(p => p.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(p => p.Name)
                   .HasMethod("GIN")
                   .HasOperators("gin_trgm_ops");

            builder.HasIndex(p => p.Description)
                   .HasMethod("GIN")
                   .HasOperators("gin_trgm_ops");

            builder.HasIndex(p => p.SKU).IsUnique();
            builder.HasIndex(p => p.Barcode).IsUnique();
        }
    }

}
