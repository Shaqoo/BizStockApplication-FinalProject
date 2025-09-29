using Application.Dto;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Wallets.GetAllWalletTransactions
{
    public record GetAllWalletTransactionsPagedQuery(PageRequest PageRequest) 
        : IRequest<Result<PaginatedList<WalletTransactionDto>>>;
   
}
