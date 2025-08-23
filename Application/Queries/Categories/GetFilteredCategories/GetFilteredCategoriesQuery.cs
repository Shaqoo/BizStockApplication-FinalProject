using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Categories.GetFilteredCategories
{
    public record GetFilteredCategoriesQuery(GetCategoriesFilter Filter)
    : IRequest<Result<PaginatedList<CategoryDto>>>;
}
