namespace Application.Validators.PurchaseOrder
{
    using Application.Dto.RequestModels;
    using FluentValidation;

    public class CreatePurchaseOrderDtoValidator : AbstractValidator<CreatePurchaseOrderDto>
    {
        public CreatePurchaseOrderDtoValidator()
        {
            RuleFor(x => x.SupplierId)
                .NotEmpty().WithMessage("SupplierId is required.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("At least one item is required.")
                .Must(items => items.Any()).WithMessage("Purchase order must contain at least one item.");

            RuleForEach(x => x.Items).SetValidator(new CreatePurchaseOrderItemDtoValidator());

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.");

            RuleFor(x => x.Tax)
                .GreaterThanOrEqualTo(0).WithMessage("Tax cannot be negative.");

            RuleFor(x => x.ExpectedDeliveryDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
                .When(x => x.ExpectedDeliveryDate.HasValue)
                .WithMessage("Expected delivery date cannot be in the past.");
        }
    }

}
