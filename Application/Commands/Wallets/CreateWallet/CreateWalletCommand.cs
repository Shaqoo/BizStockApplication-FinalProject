using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Wallets.CreateWallet
{
    public record CreateWalletCommand(CreateWalletRequest Request) : IRequest<Result<Guid>>;

}
