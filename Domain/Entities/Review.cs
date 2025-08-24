using Domain.Auditable;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review
    {
        public Guid Id { get; private init; } = Guid.NewGuid(); 
        public Guid ReviewerId { get; private set; }           
        public User Reviewer { get; private set; } = default!;

        public Guid? ProductId { get; private set; }
        public Product? Product { get; private set; }

        public Guid? SupplierId { get; private set; }
        public Supplier? Supplier { get; private set; }

        public SalesOrder Order { get; private set; } = default!;
        public Guid? OrderId { get; private set; }
        public Guid? DeliveryAgentId { get; private set; }
        public DeliveryAgent? DeliveryAgent { get; private set; }

        public int Rating { get; private set; }                   
        public string Comment { get; private set; } = string.Empty;
        public DateTime ReviewedAt { get; private set; } = DateTime.UtcNow;

        public bool IsVisible { get; private set; } = true;        
        private Review() { }

        public Review(Guid reviewerId, int rating, string comment,
                      Guid? productId = null,
                      Guid? supplierId = null,
                      Guid? deliveryAgentId = null)
        {
            if (new[] { productId, supplierId, deliveryAgentId }.Count(x => x != null) != 1)
                throw new DomainException("Review must target exactly one entity.");

            if (rating < 1 || rating > 5)
                throw new DomainException("Rating must be between 1 and 5.");

            ReviewerId = reviewerId;
            Rating = rating;
            Comment = comment;
            ProductId = productId;
            SupplierId = supplierId;
            DeliveryAgentId = deliveryAgentId;
        }

        public void AddComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                throw new DomainException("Comment cannot be empty.");
            Comment = comment;
        }

        public void Hide() => IsVisible = false;
        public void Show() => IsVisible = true;
    }

}
