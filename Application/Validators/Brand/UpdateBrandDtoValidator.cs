using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Brand
{
    public class UpdateBrandDtoValidator : AbstractValidator<UpdateBrandDto>
    {
        public UpdateBrandDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            When(x => x.Name is not null, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Name must not be empty if provided.")
                    .MaximumLength(100);
            });

            When(x => x.WebsiteUrl is not null, () =>
            {
                RuleFor(x => x.WebsiteUrl)
                    .NotEmpty().WithMessage("WebsiteUrl must not be empty if provided.")
                    .MaximumLength(255)
                    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("Invalid Website URL.");
            });

            When(x => x.LogoUrl is not null, () =>
            {
                RuleFor(x => x.LogoUrl)
                    .NotEmpty().WithMessage("LogoUrl must not be empty if provided.")
                    .MaximumLength(255)
                    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
                    .WithMessage("Invalid Logo URL.");
            });

            When(x => x.Description is not null, () =>
            {
                RuleFor(x => x.Description)
                    .MaximumLength(500);
            });
        }
    }

}
