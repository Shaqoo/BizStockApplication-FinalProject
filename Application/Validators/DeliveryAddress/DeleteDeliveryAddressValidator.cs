using Application.Commands.DeliveryAddresses.DeleteDeliveryAddress;
using FluentValidation;

namespace Application.Validators.DeliveryAddress
{
    public class DeleteDeliveryAddressValidator : AbstractValidator<DeleteDeliveryAddressCommand>
    {
        public DeleteDeliveryAddressValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Address Id is required.");
        }
    }
}
