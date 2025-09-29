using Domain.Exceptions;

namespace Domain.Entities
{
    public class DeliveryAddress
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;
        public int StateId { get; private set; }
        public State State { get; private set; } = default!;
        public int LgaId { get; private set; }
        public Lga Lga { get; private set; } = default!;
        public string Street { get; private set; } = default!;
        public string? Landmark { get; private set; }
        public string? PostalCode { get; private set; }
        public bool IsDefault { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private DeliveryAddress() {}

        private DeliveryAddress(Guid id, Guid customerId, int stateId,int lgaId, string street, bool isDefault, string? landmark, string? postalCode)
        {
            Id = id;
            CustomerId = customerId;
            StateId = stateId;
            LgaId = lgaId;
            Street = street;
            IsDefault = isDefault;
            Landmark = landmark;
            PostalCode = postalCode;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public static DeliveryAddress Create(Guid customerId,int stateId, int lgaId, string street, bool isDefault,string? landmark,string? postalCode)
        {
            if (customerId == Guid.Empty) throw new DomainException("CustomerId must be provided");
            if (stateId <= 0) throw new DomainException("StateId must be a positive integer");
            if (lgaId <= 0) throw new DomainException("LgaId must be a positive integer");
            if (string.IsNullOrWhiteSpace(street)) throw new DomainException("Street cannot be empty");

            return new DeliveryAddress(Guid.NewGuid(), customerId, stateId, lgaId, street, isDefault,landmark,postalCode);
        }

        public void UpdateStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new DomainException("Street cannot be empty.");

            Street = street;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLandmark(string? landmark)
        {
            Landmark = landmark;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePostalCode(string? postalCode)
        {
            PostalCode = postalCode;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDefault(bool isDefault)
        {
            IsDefault = isDefault;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeState(int stateId)
        {
            if (stateId <= 0) throw new DomainException("StateId must be positive");
            if (stateId == StateId) return;
            
            StateId = stateId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeLga(int lgaId)
        {
            if (lgaId <= 0) throw new DomainException("LgaId must be positive");
            if (lgaId == LgaId) return;
            
            LgaId = lgaId;
            UpdatedAt = DateTime.UtcNow;
        }
    }


}
