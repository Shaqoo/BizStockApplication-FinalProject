using Application.Dto;
using FluentValidation;

namespace Application.Validators.RecentlyViewedProduct
{
    public class AddRecentlyViewedProductRequestValidator : AbstractValidator<AddRecentlyViewedProductRequest>
    {
        public AddRecentlyViewedProductRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            RuleFor(x => x)
                .Must(x => x.UserId.HasValue || !string.IsNullOrEmpty(x.SessionId))
                .WithMessage("Either UserId or SessionId must be provided.");
        }
    }

}
