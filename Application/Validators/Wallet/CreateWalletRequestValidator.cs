namespace Application.Validators.Wallet
{
    using Application.Dto.RequestModels;
    using FluentValidation;

    public class CreateWalletRequestValidator : AbstractValidator<CreateWalletRequest>
    {
        public CreateWalletRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(x => x.Pin)
                .InclusiveBetween(1000, 9999).WithMessage("PIN must be 4 digits.");
        }
    }

}
