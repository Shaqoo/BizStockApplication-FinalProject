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
