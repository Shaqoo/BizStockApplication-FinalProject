using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Wallets.GetAllWalletTransactions
{
    public class GetAllWalletTransactionsPagedHandler(IWalletTransactionRepository walletTransactionRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetAllWalletTransactionsPagedQuery, Result<PaginatedList<WalletTransactionDto>>>
    {
        public async Task<Result<PaginatedList<WalletTransactionDto>>> Handle(GetAllWalletTransactionsPagedQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetAllWalletTransactionsPagedQuery:Page:{request.PageRequest.Page}:{request.PageRequest.PageSize}";

            var cahedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var transactions = await walletTransactionRepository.GetAllAsync(request.PageRequest);
                    return new PaginatedList<WalletTransactionDto>(transactions.Items.Select(a => a.AsDto()).ToList(),transactions.TotalCount,
                        transactions.PageNumber,transactions.PageSize);
                }, TimeSpan.FromMinutes(10));

            return Result<PaginatedList<WalletTransactionDto>>.Success(cahedResult ?? new PaginatedList<WalletTransactionDto>());
        }
    }
}
