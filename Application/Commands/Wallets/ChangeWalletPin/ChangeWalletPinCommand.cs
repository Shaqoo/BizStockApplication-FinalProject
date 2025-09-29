using Application.Dto;
using Application.Dto.RequestModels;
using MediatR;

namespace Application.Commands.Wallets.ChangeWalletPin
{
    public record ChangeWalletPinCommand(ChangeWalletPinRequest Request) : IRequest<Result<bool>>;
}
