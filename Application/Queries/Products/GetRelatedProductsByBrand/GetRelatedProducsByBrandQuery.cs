using Application.Dto;
using MediatR;

namespace Application.Queries.Products.GetRelatedProductsByBrand
{
    public record GetRelatedProductsByBrandQuery(Guid ProductId) : IRequest<Result<IEnumerable<ProductDto>>>;
}
