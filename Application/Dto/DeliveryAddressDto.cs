namespace Application.Dto
{
    public class DeliveryAddressDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty;
        public int LgaId { get; set; }
        public string LgaName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string? Landmark { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
