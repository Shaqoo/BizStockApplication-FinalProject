using Application.Dto;
using MediatR;

namespace Application.Queries.Products.GetRelatedProducts
{
    public record GetRelatedProductQuery(Guid ProductId) : IRequest<Result<IEnumerable<ProductDto>>>;
}
