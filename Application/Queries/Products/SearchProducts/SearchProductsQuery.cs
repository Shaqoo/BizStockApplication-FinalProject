using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Products.SearchProducts
{
    public record SearchProductsQuery(PageRequest PageRequest, string keyword) : IRequest<Result<PaginatedList<ProductDto>>>;
}
