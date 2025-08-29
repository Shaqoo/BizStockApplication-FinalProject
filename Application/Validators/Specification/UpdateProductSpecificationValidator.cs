using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.Specification
{
    public class UpdateProductSpecificationValidator : AbstractValidator<UpdateProductSpecificationRequest>
    {
        public UpdateProductSpecificationValidator()
        {
            RuleFor(x => x.ProductSpecificationId)
                .NotEmpty().WithMessage("Product Specification Id is required");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required")
                .MaximumLength(250);
        }
    }

}
