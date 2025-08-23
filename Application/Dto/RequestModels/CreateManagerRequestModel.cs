using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record CreateManagerRequestModel(
        string FirstName,
        string LastName,
        string Password,
        string ConfirmPassword,
        string Email,
        string PhoneNumber,
        DateTime Dob,
        Gender Gender
        );
}
