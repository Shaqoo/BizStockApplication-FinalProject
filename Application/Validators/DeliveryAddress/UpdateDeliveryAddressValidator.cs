using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.DeliveryAddress
{
    public class UpdateDeliveryAddressValidator : AbstractValidator<UpdateDeliveryAddressRequest>
    {
        public UpdateDeliveryAddressValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Address Id is required.");

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
