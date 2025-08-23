using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
