using MediatR;

namespace Domain.DomainEvents
{
    public class ProductActivatedEvent : INotification
    {
        public Guid ProductId { get; set; }
        public string ActivatedByUserName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public Guid ActivatedByUserId { get; set; }  
        public DateTime ActivatedAt { get; set; }    
    }

}
