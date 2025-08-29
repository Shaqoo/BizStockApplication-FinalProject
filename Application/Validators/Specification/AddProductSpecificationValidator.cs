using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.Specification
{
    public class AddProductSpecificationValidator : AbstractValidator<AddProductSpecificationRequest>
    {
        public AddProductSpecificationValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product Id is required");

            RuleFor(x => x.SpecificationId)
                .NotEmpty().WithMessage("Specification Id is required");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required")
                .MaximumLength(250);
        }
    }

}
