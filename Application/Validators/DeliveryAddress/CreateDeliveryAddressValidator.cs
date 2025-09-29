namespace Application.Validators.DeliveryAddress
{
    using Application.Dto.RequestModels;
    using FluentValidation;

    public class CreateDeliveryAddressValidator : AbstractValidator<CreateDeliveryAddressRequest>
    {
        public CreateDeliveryAddressValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(x => x.StateId)
                .GreaterThan(0).WithMessage("StateId must be valid.");

            RuleFor(x => x.LgaId)
                .GreaterThan(0).WithMessage("LgaId must be valid.");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(200).WithMessage("Street cannot exceed 200 characters.");

            RuleFor(x => x.PostalCode)
                .MaximumLength(20).WithMessage("PostalCode cannot exceed 20 characters.");
        }
    }
}
