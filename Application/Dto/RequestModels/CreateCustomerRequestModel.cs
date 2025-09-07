using Domain.Enums;

namespace Application.Dto.RequestModels
{
    public record CreateCustomerRequestModel(
        string FirstName,
        string LastName,
        string Password,
        string ConfirmPassword,
        string Email,
        string PhoneNumber,
        string? Address,
        string? BusinessName,
        string? State,
        string? TaxId,
        DateTime BirthDate,
        Gender Gender,
        CustomerTypeName CustomerType,
        int Pin
    );

}
