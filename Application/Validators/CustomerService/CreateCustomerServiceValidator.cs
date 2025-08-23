using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.CustomerService
{
    public class CreateCustomerServiceValidator : AbstractValidator<CreateCustomerRequestModel>
    {
        public CreateCustomerServiceValidator()
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

            RuleFor(x => x.BirthDate)
               .NotEmpty().WithMessage("Date of birth is required.")
               .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.")
               .Must(date => date <= DateTime.Now.AddYears(-18))
               .WithMessage("You must be at least 18 years old.");

        }
        private bool IsValidName(string name)
        {
            return name.All(char.IsLetter);
        }
    }
}
