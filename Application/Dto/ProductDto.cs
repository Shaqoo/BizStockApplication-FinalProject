using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record ProductDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public string SKU { get; init; } = default!;
        public string Barcode { get; init; } = default!;
        public string QrCodeValue { get; init; } = default!;
        public string Description { get; init; } = default!;
        public string ImageUrl { get; init; } = default!;
        public decimal CostPrice { get; init; }
        public decimal SellingPrice { get; init; }
        public string UnitOfMeasure { get; init; } = default!;
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = default!;
        public Guid BrandId { get; init; }
        public string BrandName { get; init; } = default!;
        public int Quantity { get; init; } = default!;
        public int ReorderLevel { get; init; } = default!;
    }


}
