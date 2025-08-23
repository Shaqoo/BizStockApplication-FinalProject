using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.RequestModels
{
    public class CreateProductReviewDto
    {
        public required Guid ProductId { get; init; }
        public required int Rating { get; init; }
        public string? Comment { get; init; }  
    }

}
