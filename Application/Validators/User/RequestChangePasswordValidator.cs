using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators.User
{
    public class RequestChangePasswordValidator : AbstractValidator<RequestChangePasswordDto>
    {
        public RequestChangePasswordValidator()
        {
            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
