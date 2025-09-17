using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class CancelPurchaseOrderDtoValidator : AbstractValidator<CancelPurchaseOrderDto>
    {
        public CancelPurchaseOrderDtoValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Cancellation reason is required.")
                .MaximumLength(300).WithMessage("Reason cannot exceed 300 characters.");
        }
    }

}
