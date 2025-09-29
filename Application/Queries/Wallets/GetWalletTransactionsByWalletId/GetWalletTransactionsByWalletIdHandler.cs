using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Wallets.GetWalletTransactionsByWalletId
{
    public class GetWalletTransactionsByWalletIdHandler(IWalletTransactionRepository walletTransactionRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetWalletTransactionsByWalletIdQuery, Result<PaginatedList<WalletTransactionDto>>>
    {
        public async Task<Result<PaginatedList<WalletTransactionDto>>> Handle(GetWalletTransactionsByWalletIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"GetWalletTransactionsByWalletIdQuery:{request.walletId}:Page:{request.PageRequest.Page}:{request.PageRequest.PageSize}";

            var cahedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var transactions = await walletTransactionRepository.GetByWalletPagedAsync(request.walletId, request.PageRequest);
                    return transactions;
                },TimeSpan.FromMinutes(5));

            return Result<PaginatedList<WalletTransactionDto>>.Success(cahedResult ?? new PaginatedList<WalletTransactionDto>());
        }
    }
}
