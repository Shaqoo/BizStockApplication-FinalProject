using Domain.Auditable;
using Domain.Enums;
using Domain.Exceptions;
using NpgsqlTypes;

namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string SKU { get; private set; } = default!;
        public string Barcode { get; private set; } = default!;
        public string QrCodeValue { get; private set; } = default!;
        public string? Description { get; private set; }
        public string ImageUrl { get; private set; } = default!;

        public Brand Brand { get; private set; } = default!;    
        public Guid BrandId { get; private set; }

        public ProductStatus Status { get; private set; } = ProductStatus.Pending;

        public string? ApprovedBy { get; private set; } = default!;
        public DateTimeOffset? ArchivedAt { get; set; }
        public NpgsqlTsVector SearchVector { get; private set; } = default!;
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = default!;

        public decimal CostPrice { get; private set; }
        public decimal SellingPrice { get; private set; }

        public ProductType Type { get; private set; } = ProductType.FinishedGood;

        public UnitOfMeasure UnitOfMeasure { get; private set; }
        public bool IsActive { get; private set; } = true;
        public ICollection<Review> Reviews { get; private set; } = new HashSet<Review>();
        public ICollection<WarehouseItem> StockByWarehouse { get; private set; } = new HashSet<WarehouseItem>();
        public ICollection<ProductTag> ProductTags { get; private set; } = new HashSet<ProductTag>();
        public ICollection<CartItem> CartItems { get; private set; } = new HashSet<CartItem>();
        public ICollection<WishlistItem> WishlistItems { get; private set; } = new HashSet<WishlistItem>();
        public ICollection<RecentlyViewedProduct> RecentlyViewedProducts { get; private set; } = new HashSet<RecentlyViewedProduct>();
        public ICollection<ProductSpecification> ProductSpecifications { get; private set; } = new HashSet<ProductSpecification>();
        private Product() { }

        public Product(
            string name,
            string sku,
            string barcode,
            string imageUrl,
            Guid categoryId,
            decimal costPrice,
            decimal sellingPrice,
            UnitOfMeasure unitOfMeasure,
            Guid brandId,
            string qrCodeValue,
            string? description = null
        )
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Product name is required.");
            if (string.IsNullOrWhiteSpace(sku)) throw new DomainException("SKU is required.");
            if (string.IsNullOrWhiteSpace(barcode)) throw new DomainException("Barcode is required.");
            if (string.IsNullOrWhiteSpace(imageUrl)) throw new DomainException("Image URL is required.");
            if (costPrice < 0 || sellingPrice < 0) throw new DomainException("Prices must be non-negative.");

            Name = name;
            SKU = sku;
            Barcode = barcode;
            ImageUrl = imageUrl;
            CategoryId = categoryId;
            CostPrice = costPrice;
            SellingPrice = sellingPrice;
            UnitOfMeasure = unitOfMeasure;
            Description = description;
            QrCodeValue = qrCodeValue;
            BrandId = brandId;
        }

        public void UpdatePrices(decimal cost, decimal selling)
        {
            if (cost < 0 || selling < 0)
                throw new DomainException("Prices must be non-negative.");

            CostPrice = cost;
            SellingPrice = selling;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty.");
            Name = name;
        }

        public void UpdateUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        {
            if (!Enum.IsDefined(typeof(UnitOfMeasure), unitOfMeasure))
                throw new DomainException("Invalid unit of measure.");
            UnitOfMeasure = unitOfMeasure;
        }

        public void UpdateQrCodeValue(string qrCodeValue)
        {
            QrCodeValue = qrCodeValue;
        }

        public void SetPicture(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new DomainException("Image URL cannot be empty.");
            ImageUrl = imageUrl;
        }

        public void Archive()
        {
            IsActive = false;
            ArchivedAt = DateTimeOffset.UtcNow;
            Status = ProductStatus.Archived;
        }

        public void Reactivate()
        {
            IsActive = true;
            if (Status == ProductStatus.Archived)
                Status = ProductStatus.Pending;
        }

        public void Approve(string approvedBy)
        {
            if (string.IsNullOrWhiteSpace(approvedBy))
                throw new DomainException("Approved by cannot be empty.");
            Status = ProductStatus.Approved;
            ApprovedBy = approvedBy;
        }

        public void Reject(string approvedBy)
        {
            if (string.IsNullOrWhiteSpace(approvedBy))
                throw new DomainException("Approved by cannot be empty.");
            Status = ProductStatus.Rejected;
            ApprovedBy = approvedBy;
            IsActive = false;
        }

        public void Activate()
        {
            ArchivedAt = null;
            Status = ProductStatus.Approved;
        }

        public void AddTag(ProductTag tag)
        {
            if (tag == null) throw new DomainException("Tag cannot be null.");
            if (ProductTags.Any(t => t.TagId == tag.Id)) return;  
            ProductTags.Add(tag);
        }

    }


}
