using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.User
{
    public class VerifyPasswordResetValidator : AbstractValidator<VerifyPasswordReset>
    {
        public VerifyPasswordResetValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code Cannot Be Empty");
        }
    }
}
