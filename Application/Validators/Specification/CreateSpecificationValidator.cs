using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.Specification
{
    public class CreateSpecificationValidator : AbstractValidator<CreateSpecificationRequest>
    {
        public CreateSpecificationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Specification name is required")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(2000);
        }
    }

}
