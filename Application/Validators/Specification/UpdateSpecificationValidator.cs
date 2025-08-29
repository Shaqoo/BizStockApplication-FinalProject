using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.Specification
{
    public class UpdateSpecificationValidator : AbstractValidator<UpdateSpecificationRequest>
    {
        public UpdateSpecificationValidator()
        {
            RuleFor(x => x.SpecificationId)
                .NotEmpty().WithMessage("Specification Id is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Specification name is required")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(2000);
        }
    }

}
