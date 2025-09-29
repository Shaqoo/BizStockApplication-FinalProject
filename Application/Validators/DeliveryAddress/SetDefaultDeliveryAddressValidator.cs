using Application.Commands.DeliveryAddresses.SetDefaultDeliveryAddress;
using FluentValidation;

namespace Application.Validators.DeliveryAddress
{
    public class SetDefaultDeliveryAddressValidator : AbstractValidator<SetDefaultDeliveryAddressCommand>
    {
        public SetDefaultDeliveryAddressValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(x => x.AddressId)
                .NotEmpty().WithMessage("Address Id is required.");
        }
    }
}
