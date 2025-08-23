using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validations.Product
{
    public class AddProductReviewDtoValidator : AbstractValidator<CreateProductReviewDto>
    {
        public AddProductReviewDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();

            RuleFor(x => x.Rating).InclusiveBetween(1, 5);

            RuleFor(x => x.Comment).MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Comment));
        }
    }

}
