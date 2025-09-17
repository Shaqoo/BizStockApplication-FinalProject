using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class UpdatePurchaseOrderDtoValidator : AbstractValidator<UpdatePurchaseOrderDto>
    {
        public UpdatePurchaseOrderDtoValidator()
        {
            RuleFor(x => x.PurchaseOrderId)
                .NotEmpty().WithMessage("Purchase Order ID is required.");

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");

            RuleFor(x => x.Tax)
                .GreaterThanOrEqualTo(0).WithMessage("Tax cannot be negative.");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.");
        }
    }

}
