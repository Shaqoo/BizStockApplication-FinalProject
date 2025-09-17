using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class UpdatePurchaseOrderItemDtoValidator : AbstractValidator<UpdatePurchaseOrderItemDto>
    {
        public UpdatePurchaseOrderItemDtoValidator()
        {
            RuleFor(x => x.PurchaseOrderId)
                .NotEmpty().WithMessage("Purchase Order ID is required.");

            RuleFor(x => x.PurchaseOrderItemId)
                .NotEmpty().WithMessage("Purchase Order Item ID is required.");

            RuleFor(x => x.QuantityOrdered)
                .GreaterThan(0).WithMessage("Quantity ordered must be greater than zero.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }

}
