using Application.Dto;
using MediatR;

namespace Application.Queries.Products.GetByIds
{
    public record GetProductByIdsQuery(List<Guid> Ids) : IRequest<IEnumerable<ProductDto>>;
}
