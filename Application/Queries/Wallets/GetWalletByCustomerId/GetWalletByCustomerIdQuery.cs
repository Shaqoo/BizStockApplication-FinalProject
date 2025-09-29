using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Queries.Wallets.GetWalletByCustomerId
{
    public record GetWalletByCustomerIdQuery(Guid CustomerId)
    : IRequest<Result<WalletDto>>;

}
