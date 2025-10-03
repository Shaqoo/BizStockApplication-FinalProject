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

            RuleFor(x => x.Landmark)
               .MaximumLength(100).WithMessage("Landmark cannot exceed 100 characters.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("IsDefault must be specified.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.PhoneNumber)
               .NotEmpty().WithMessage("Phone number is required.")
               .Matches(@"^\+?\d{10,15}$").WithMessage("Enter a valid phone number.");

            RuleFor(RuleFor => RuleFor.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.");

            RuleFor(x => x.AdditionalPhoneNumber)
                .MaximumLength(15).WithMessage("Additional phone number cannot exceed 15 characters.")
                .Matches(@"^\+?\d{10,15}$").When(x => !string.IsNullOrEmpty(x.AdditionalPhoneNumber)).WithMessage("Enter a valid additional phone number.");
        }
    }

}
