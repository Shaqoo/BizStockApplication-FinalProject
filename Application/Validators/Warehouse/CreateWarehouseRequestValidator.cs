using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Warehouse
{
    public class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseDto>
    {
        public CreateWarehouseRequestValidator()
        {
            RuleFor(x  => x.Name).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
        }
    }
}
