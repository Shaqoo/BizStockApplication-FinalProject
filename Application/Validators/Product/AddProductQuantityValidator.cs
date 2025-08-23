using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validations.Product
{
    public class AddProductQuantityDtoValidator : AbstractValidator<AddProductQuantityDto>
    {
        public AddProductQuantityDtoValidator()
        {
            RuleFor(x => x.WarehouseId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ReorderLevel).GreaterThanOrEqualTo(0);
        }
    }

}
