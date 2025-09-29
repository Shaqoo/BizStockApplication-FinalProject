using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.Wallet
{
    public class ChangeWalletPinRequestValidator : AbstractValidator<ChangeWalletPinRequest>
    {
        public ChangeWalletPinRequestValidator()
        {
            RuleFor(x => x.WalletId)
                .NotEmpty().WithMessage("WalletId is required.");

            RuleFor(x => x.OldPin)
                .InclusiveBetween(1000, 9999).WithMessage("Old PIN must be 4 digits.");

            RuleFor(x => x.NewPin)
                .InclusiveBetween(1000, 9999).WithMessage("New PIN must be 4 digits.")
                .NotEqual(x => x.OldPin).WithMessage("New PIN must be different from Old PIN.");
        }
    }

}
