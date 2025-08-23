using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Category
{
    public class MoveCategoryDtoValidator : AbstractValidator<MoveCategoryDto>
    {
        public MoveCategoryDtoValidator()
        {
            RuleFor(x => x.NewParentCategoryId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
