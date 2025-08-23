using Domain.Auditable;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class DeliveryAgent : BaseEntity
    {
        public string FullName { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public string VehicleNumber { get; private set; } = default!;
        public string? ContactNumber { get; private set; }
        public DeliveryAvailabilityStatus AvailabilityStatus { get; private set; } = DeliveryAvailabilityStatus.Available;

        private HashSet<DeliveryAssignment> _Assignments = new();
        public IReadOnlyCollection<DeliveryAssignment> Assignments => _Assignments;
        public ICollection<Review> Reviews { get; private set; } = new HashSet<Review>();

        private DeliveryAgent() { }

        public DeliveryAgent(string fullName,Email email, string vehicleNumber, string? contactNumber = null)
        {
            if (string.IsNullOrWhiteSpace(vehicleNumber))
                throw new DomainException("Vehicle number is required.");

            FullName = fullName;
            Email = email;
            VehicleNumber = vehicleNumber;
            ContactNumber = contactNumber;
        }

        public void UpdateContact(string newNumber)
        {
            if (string.IsNullOrWhiteSpace(newNumber))
                throw new DomainException("Contact number cannot be empty.");

            ContactNumber = newNumber;
            Modified();
        }

        public void AssignDelivery(DeliveryAssignment assignment)
        {
            if (assignment == null) throw new DomainException(nameof(assignment));
            _Assignments.Add(assignment);
        }

        public void MarkAsAvailable() => AvailabilityStatus = DeliveryAvailabilityStatus.Available;

        public void MarkAsBusy() => AvailabilityStatus = DeliveryAvailabilityStatus.Busy;

        public void MarkAsOffline() => AvailabilityStatus = DeliveryAvailabilityStatus.Offline;
    }

}
