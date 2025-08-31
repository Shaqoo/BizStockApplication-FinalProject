using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Products.GetProductsByBrand
{
    public record GetProductsByBrandQuery(Guid brandId,PageRequest PageRequest) : IRequest<Result<PaginatedList<ProductDto>>>;
    
}
