using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class ProductTag
    {
        public Guid Id { get;private set; } = Guid.NewGuid();
        public Product Product { get; private set; } = default!;
        public Guid ProductId { get; private set; }
        public Tag Tag { get; private set; } = default!;
        public Guid TagId { get; private set; }

        private ProductTag() { }
        public ProductTag(Guid productId,Guid tagId)
        {
            ProductId = productId;
            TagId = tagId;
        }
    }
}
