using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class CreatePurchaseOrderItemDtoValidator : AbstractValidator<CreatePurchaseOrderItemDto>
    {
        public CreatePurchaseOrderItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(200);

            RuleFor(x => x.QuantityOrdered)
                .GreaterThan(0).WithMessage("Quantity ordered must be greater than 0.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        }
    }

}
