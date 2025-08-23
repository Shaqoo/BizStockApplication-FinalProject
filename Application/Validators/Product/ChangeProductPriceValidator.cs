using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Product
{
    public class ChangeProductPriceValidator : AbstractValidator<ChangeProductPriceDto>
    {
        public ChangeProductPriceValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required.");
            
            RuleFor(x => x.CostPrice)
                .GreaterThan(0)
                .WithMessage("New price must be greater than zero.");

            RuleFor(x => x.SellingPrice)
                .GreaterThan(0)
                .WithMessage("New price must be greater than zero.");
        }
    }
}
