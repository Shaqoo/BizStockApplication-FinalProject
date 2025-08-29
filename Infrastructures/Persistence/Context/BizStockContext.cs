using Domain.Entities;
using Domain.Entities.Domain.Entities;
using Infrastructures.Configuration.EntityTypeConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Persistence.Context
{
    public class BizStockContext(DbContextOptions<BizStockContext> options) : DbContext(options)
    {
        public DbSet<ProductSpecification> ProductSpecifications { get; set; } = default!;
        public DbSet<Specification> Specifications { get; set; } = default!;
        public DbSet<Cart> Carts { get; set; } = default!;
        public DbSet<CartItem> CartItems { get; set; } = default!;
        public DbSet<Wishlist> Wishlists { get; set; } = default!;
        public DbSet<WishlistItem> WishlistItems { get; set; } = default!;
        public DbSet<RecentlyViewedProducts> RecentlyViewedProducts { get; set; } = default!;
        public DbSet<RecentlyViewedProduct> RecentlyViewedProductsItems { get; set; } = default!;
        public DbSet<LostAccessRequest> LostAccessRequests { get; set; } = default!;
        public DbSet<UserRecoveryCode> UserRecoveryCodes { get; set; } = default!;
        public DbSet<Tag> Tags { get; set; } = default!;
        public DbSet<ProductTag> ProductTags { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Brand> Brands { get; set; } = default!;
        public DbSet<SalesOrder> SalesOrders { get; set; } = default!;
        public DbSet<SalesOrderItem> SalesOrderItems { get; set; } = default!;
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = default!;
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = default!;
        public DbSet<WarehouseItem> WarehouseItems { get; set; } = default!;
        public DbSet<Warehouse> Warehouses { get; set; } = default!;
        public DbSet<Payment> Payments { get; set; } = default!;
        public DbSet<Wallet> Wallets { get; set; } = default!;
        public DbSet<WalletTransaction> WalletTransactions { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Invoice> Invoices { get; set; } = default!;
        public DbSet<InvoiceItem> InvoiceItems { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Supplier> Suppliers { get; set; } = default!;
        public DbSet<CustomerType> CustomerTypes { get; set; } = default!;
        public DbSet<DeliveryAgent> DeliveryAgents { get; set; } = default!;
        public DbSet<DeliveryAssignment> DeliveryAssignments { get; set; } = default!;
        public DbSet<DeliveryFeeRule> DeliveryFeeRules { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<FidoCredential> FidoCredentials { get; set; } = default!;
        public DbSet<StockMovement> StockMovements { get; set; } = default!;
        public DbSet<ChatMessage> ChatMessages { get; set; } = default!;
        public DbSet<ChatThread> ChatThreads { get; set; } = default!;
        public DbSet<MessageReaction> MessageReactions { get; set; } = default!;
        public DbSet<Notification> Notifications { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
