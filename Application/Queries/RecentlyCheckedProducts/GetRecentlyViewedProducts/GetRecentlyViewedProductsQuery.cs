using Application.Dto;
using MediatR;

namespace Application.Queries.RecentlyCheckedProducts.GetRecentlyViewedProducts
{
    public record GetRecentlyViewedProductsQuery(Guid? UserId, string? SessionId)
     : IRequest<Result<RecentlyViewedProductsDto>>;
}
