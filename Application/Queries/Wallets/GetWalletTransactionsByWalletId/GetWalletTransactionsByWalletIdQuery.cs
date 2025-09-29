using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Wallets.GetWalletTransactionsByWalletId
{
    public record GetWalletTransactionsByWalletIdQuery(Guid walletId,PageRequest PageRequest)
        : IRequest<Result<PaginatedList<WalletTransactionDto>>>;
}
