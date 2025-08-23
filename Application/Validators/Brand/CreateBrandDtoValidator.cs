using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.RequestModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Application.Validations.Brand
{
    public class CreateBrandDtoValidator : AbstractValidator<CreateBrandDto>
    {
        public CreateBrandDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand name is required.")
                .MaximumLength(100).WithMessage("Brand name must not exceed 100 characters.");

            RuleFor(x => x.WebsiteUrl)
                .NotEmpty().WithMessage("Website URL is required.")
                .Must(BeAValidUrl).WithMessage("Website URL is not valid.");

            RuleFor(x => x.LogoUrl)
                .NotEmpty().WithMessage("Logo URL is required.")
                .Must(BeAValidUrl).WithMessage("Logo URL is not valid.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var result)
                   && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
        }
    }

}
