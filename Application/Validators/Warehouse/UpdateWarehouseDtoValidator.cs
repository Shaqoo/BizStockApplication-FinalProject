using Application.Dto.RequestModels;
using FluentValidation;
namespace Application.Validations.Warehouse
{
    public class UpdateWarehouseDtoValidator : AbstractValidator<UpdateWarehouseDto>
    {
        public UpdateWarehouseDtoValidator()
        {
            RuleFor(x => new { x.Name, x.Location })
                .Must(x => !string.IsNullOrWhiteSpace(x.Name) || !string.IsNullOrWhiteSpace(x.Location))
                .WithMessage("Name Or Location Must Be Provided");
        }
    }
}
