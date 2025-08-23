using Application.Dto;
using MediatR;

namespace Application.Commands.RecentlyCheckedProduct.AddRecentlyViewedProduct
{
    public record AddRecentlyViewedProductCommand(AddRecentlyViewedProductRequest Request)
    : IRequest<Result<string>>;
}
