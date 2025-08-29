using Application.Dto;
using MediatR;

namespace Application.Queries.Specifications.GetProductSpecificationsByProductId
{
    public record GetProductSpecificationsByProductIdQuery(Guid ProductId) : IRequest<Result<ProductSpecificationListDto>>;

}
