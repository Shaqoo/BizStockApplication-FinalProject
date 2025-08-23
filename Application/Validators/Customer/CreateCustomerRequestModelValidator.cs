using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Customer
{
    using Application.Dto.RequestModels;
    using Domain.Enums;
    using FluentValidation;

    public class CreateCustomerRequestModelValidator : AbstractValidator<CreateCustomerRequestModel>
    {
        public CreateCustomerRequestModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8)
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Enter a valid phone number.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Invalid gender selection.");

            RuleFor(x => x.CustomerType)
                .IsInEnum().WithMessage("Invalid customer type.");

            RuleFor(x => x.BirthDate)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.")
                .Must(date => date <= DateTime.Now.AddYears(-13))
                .WithMessage("You must be at least 13 years old.");

            RuleFor(x => x.Pin)
                .InclusiveBetween(1000, 9999)
                    .WithMessage("Pin must be exactly 4 digits.");


            When(x => x.CustomerType is CustomerTypeName.Wholesale
                                   or CustomerTypeName.Corporate
                                   or CustomerTypeName.Reseller, () =>
                                   {
                                       RuleFor(x => x.BusinessName)
                                       .NotEmpty().WithMessage("Business name is required for business customers.");

                                       RuleFor(x => x.TaxId)
                                       .NotEmpty().WithMessage("Tax ID is required for business customers.");

                                       RuleFor(x => x.State)
                                       .NotEmpty().WithMessage("State is required for business customers.");

                                        RuleFor(x => x.Address)
                                        .NotEmpty().WithMessage("Address is required for business customers.");
                                   });
        }
        private bool IsValidName(string name)
        {
            return name.All(char.IsLetter);
        }
    }

}
