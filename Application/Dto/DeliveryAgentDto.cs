using Domain.Enums;

namespace Application.Dto
{
    public record DeliveryAgentDto(Guid Id,string Fullname,string Email,string VehicleNo,string Contact,
        DeliveryAvailabilityStatus AvailabilityStatus);
    
}
