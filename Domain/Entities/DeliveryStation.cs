using Domain.Auditable;

namespace Domain.Entities
{
    public class DeliveryStation : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public int StateId { get; private set; }
        public State State { get; private set; } = default!;
        public int LgaId { get; private set; }
        public Lga Lga { get; private set; } = default!;
        public decimal BaseFee { get; private set; }    
        public decimal FeePerKm { get; private set; } 
        public bool IsActive { get; private set; } = true;
        private readonly List<DeliveryAddress> _addresses = new();
        public IReadOnlyCollection<DeliveryAddress> Addresses => _addresses.AsReadOnly();

        private DeliveryStation () { }

        public DeliveryStation(string name, int stateId, int lgaId, decimal baseFee, decimal feePerKm)
        {
            Name = name;
            StateId = stateId;
            LgaId = lgaId;
            BaseFee = baseFee;
            FeePerKm = feePerKm;
        }
    }

}
