using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class AddPurchaseOrderItemDtoValidator : AbstractValidator<AddPurchaseOrderItemDto>
    {
        public AddPurchaseOrderItemDtoValidator()
        {
            RuleFor(x => x.PurchaseOrderId)
                .NotEmpty().WithMessage("Purchase Order ID is required.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

            RuleFor(x => x.QuantityOrdered)
                .GreaterThan(0).WithMessage("Quantity ordered must be greater than zero.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }

}
