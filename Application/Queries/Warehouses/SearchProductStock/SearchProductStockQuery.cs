using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Warehouses.SearchProductStock
{
    public record SearchProductStockQuery(string Keyword,PageRequest PageRequest) 
        : IRequest<Result<PaginatedList<ProductStockSummaryDto>>>;

}
