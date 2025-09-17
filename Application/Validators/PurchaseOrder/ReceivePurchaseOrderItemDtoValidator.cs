using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class ReceivePurchaseOrderItemDtoValidator : AbstractValidator<ReceivePurchaseOrderItemDto>
    {
        public ReceivePurchaseOrderItemDtoValidator()
        {
            RuleFor(x => x.PurchaseOrderItemId)
                .NotEmpty().WithMessage("Purchase order item id is required.");

            RuleFor(x => x.QuantityReceived)
                .GreaterThan(0)
                .WithMessage("Quantity received must be greater than 0.");
        }
    }

}
