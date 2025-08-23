using Application.Dto;
using MediatR;

namespace Application.Commands.RecentlyCheckedProduct.RemoveRecentlyViewedProduct
{
    public record RemoveRecentlyViewedProductCommand(string? sessionId, Guid ProductId) : IRequest<Result<Unit>>;

}
