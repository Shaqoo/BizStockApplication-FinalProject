using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public record CategoryDto
    (
    Guid Id,
    string Name,
    string? Description,
    int Depth,
    Guid? ParentCategoryId,
    int ProductCount
    );

}
