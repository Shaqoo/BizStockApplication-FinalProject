using Application.Dto;
using MediatR;

namespace Application.Commands.RecentlyCheckedProduct.ClearRecentlyViewedProducts
{
    public record ClearRecentlyViewedProductsCommand(string? SessionId) : IRequest<Result<Unit>>;
}
