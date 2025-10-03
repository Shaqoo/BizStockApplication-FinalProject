namespace Application.Dto.RequestModels
{
    public class CreateDeliveryAddressRequest
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AdditionalPhoneNumber { get; set; } = string.Empty;
        public int StateId { get; set; }
        public int LgaId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string? Landmark { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateDeliveryAddressRequest
    {
        public Guid Id { get; set; }
        public int StateId { get; set; }
        public int LgaId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string? Landmark { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? AdditionalPhoneNumber { get; set; } = string.Empty;
    }

}
