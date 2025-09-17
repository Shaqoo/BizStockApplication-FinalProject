using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class ConfirmPurchaseOrderDtoValidator : AbstractValidator<ConfirmPurchaseOrderDto>
    {
        public ConfirmPurchaseOrderDtoValidator()
        {
            RuleFor(x => x.ExpectedDeliveryDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Expected delivery date must be in the future.");

            RuleFor(x => x.Notes)
                .MaximumLength(500)
                .WithMessage("Notes cannot exceed 500 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        }
    }

}
