using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class RemovePurchaseOrderItemDtoValidator : AbstractValidator<RemovePurchaseOrderItemDto>
    {
        public RemovePurchaseOrderItemDtoValidator()
        {
            RuleFor(x => x.PurchaseOrderId)
                .NotEmpty().WithMessage("Purchase Order ID is required.");

            RuleFor(x => x.PurchaseOrderItemId)
                .NotEmpty().WithMessage("Purchase Order Item ID is required.");
        }
    }

}
