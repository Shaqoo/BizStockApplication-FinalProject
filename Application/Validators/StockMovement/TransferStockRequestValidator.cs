using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validations.StockMovement
{
    public class TransferStockRequestValidator : AbstractValidator<TransferStockRequest>
    {
        public TransferStockRequestValidator()
        {
            RuleFor(request => request.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Product ID cannot be empty.");

            RuleFor(request => request.FromWarehouseId)
                .NotEmpty().WithMessage("From Warehouse ID is required.")
                .NotEqual(Guid.Empty).WithMessage("From Warehouse ID cannot be empty.")
                .NotEqual(request => request.ToWarehouseId)
                .WithMessage("Source and destination warehouses must be different."); ;

            RuleFor(request => request.ToWarehouseId)
                .NotEmpty().WithMessage("To Warehouse ID is required.")
                .NotEqual(Guid.Empty).WithMessage("To Warehouse ID cannot be empty.");

            RuleFor(request => request.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(request => request.Reason)
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.")
                .When(request => !string.IsNullOrEmpty(request.Reason));
        }
    }
}
