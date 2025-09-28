using Domain.Enums;

namespace Application.Dto.RequestModels
{
    public record CreateDeliveryAgentModel(
        string FirstName,
        string LastName,
        string Password,
        string ConfirmPassword,
        string Email,
        string PhoneNumber,
        DateTime Dob,
        Gender Gender,
        string VehicleNumber
    );

}
