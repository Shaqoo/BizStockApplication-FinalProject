using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Product
{
    public class ReviewCreatedProductDtoValidator : AbstractValidator<ReviewCreatedProductDto>
    {
        public ReviewCreatedProductDtoValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }

}
