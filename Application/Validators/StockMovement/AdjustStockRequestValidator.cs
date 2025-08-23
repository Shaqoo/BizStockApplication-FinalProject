using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.StockMovement
{
    public class AdjustStockRequestValidator : AbstractValidator<AdjustStockRequest>
    {
        public AdjustStockRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Product ID cannot be empty.");

            RuleFor(x => x.WarehouseId)
                .NotEmpty().WithMessage("Warehouse ID is required.")
                .NotEqual(Guid.Empty).WithMessage("Warehouse ID cannot be empty.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.Reason)
                .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters.")
                .NotEmpty().WithMessage("Reason For AdjustMent Can't Be Null")
                .NotNull().WithMessage("Manual Stock Adjustment Reason Can't Be Null");

            RuleFor(x => x.AdjustmentType)
                .IsInEnum().WithMessage("Invalid adjustment type. Must be either Increase or Decrease.");
        }
    }
}
