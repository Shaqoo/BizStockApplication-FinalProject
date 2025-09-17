using Application.Commands.PurchaseOrders.ReceivePurchaseOrderItems;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class ReceivePurchaseOrderItemsCommandValidator : AbstractValidator<ReceivePurchaseOrderItemsCommand>
    {
        public ReceivePurchaseOrderItemsCommandValidator()
        {
            RuleFor(x => x.PurchaseOrderId)
                .NotEmpty().WithMessage("Purchase order id is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new ReceivePurchaseOrderItemDtoValidator());
        }
    }

}
