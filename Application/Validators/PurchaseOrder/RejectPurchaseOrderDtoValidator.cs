using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.PurchaseOrder
{
    public class RejectPurchaseOrderDtoValidator : AbstractValidator<RejectPurchaseOrderDto>
    {
        public RejectPurchaseOrderDtoValidator()
        {
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Rejection reason is required.")
                .MaximumLength(300).WithMessage("Reason cannot exceed 300 characters.");
        }
    }

}
