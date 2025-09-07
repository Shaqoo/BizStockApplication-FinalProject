using Domain.Entities;
using Domain.Entities.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.Configuration.EntityTypeConfiguration
{

    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId).IsRequired(false);

            builder.HasMany(c => c.Items)
                   .WithOne(ci => ci.Cart)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }


    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.ProductId).IsRequired();
            builder.Property(ci => ci.Quantity).IsRequired();

            builder.HasOne(ci => ci.Cart)
                   .WithMany(c => c.Items)
                   .HasForeignKey(ci => ci.CartId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }


    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.UserId).IsRequired();
        }
    }

    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.HasKey(wi => wi.Id);

            builder.Property(wi => wi.ProductId).IsRequired();
        }
    }

    public class RecentlyViewedProductsConfiguration : IEntityTypeConfiguration<RecentlyViewedProducts>
    {
        public void Configure(EntityTypeBuilder<RecentlyViewedProducts> builder)
        {
            builder.HasKey(rv => rv.Id);

            builder.Property(rv => rv.UserId).IsRequired(false);
            builder.Property(rv => rv.SessionId).HasMaxLength(100).IsRequired(false);
            builder.Property(rv => rv.IsLinked).IsRequired();
        }
    }

    public class RecentlyViewedProductConfiguration : IEntityTypeConfiguration<RecentlyViewedProduct>
    {
        public void Configure(EntityTypeBuilder<RecentlyViewedProduct> builder)
        {
            builder.HasKey(rvp => rvp.Id);

            builder.Property(rvp => rvp.ProductId).IsRequired();
           
        }
    }
}
