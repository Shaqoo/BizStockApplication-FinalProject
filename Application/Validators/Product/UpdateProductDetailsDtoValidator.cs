using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validations.Product
{
    public class UpdateProductDetailsDtoValidator : AbstractValidator<UpdateProductDetailsDto>
    {
        public UpdateProductDetailsDtoValidator()
        {
            RuleFor(dto => dto)
                .Must(dto => !string.IsNullOrWhiteSpace(dto.Name) ||
                             !string.IsNullOrWhiteSpace(dto.Description))
                .WithMessage("At least one field (Name, Description, UnitOfMeasure) must be provided.");

            When(dto => dto.Name != null, () =>
            {
                RuleFor(dto => dto.Name).MaximumLength(150);
            });

            When(dto => dto.Description != null, () =>
            {
                RuleFor(dto => dto.Description).MaximumLength(1000);
            });

           
            RuleFor(dto => dto.UnitOfMeasure).IsInEnum();
     
        }
    }

}
