using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Products.AddProductReview
{
    public record AddProductReviewCommand(CreateProductReviewDto Dto, RequestMetadata RequestMetadata)
    : IRequest<Result<Guid>>;

}
