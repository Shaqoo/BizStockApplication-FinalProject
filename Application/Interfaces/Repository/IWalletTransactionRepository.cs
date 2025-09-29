using Application.Dto;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IWalletTransactionRepository : IBaseRepository<WalletTransaction>
    {
        Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId);
        Task<IEnumerable<WalletTransaction>> GetByUserIdAsync(Guid userId); 
        Task<WalletTransaction?> GetByReferenceAsync(string reference);
        Task<PaginatedList<WalletTransactionDto>> GetByWalletPagedAsync(Guid walletId, PageRequest pageRequest);
        Task<decimal> GetTotalCreditsAsync(Guid walletId);
        Task<decimal> GetTotalDebitsAsync(Guid walletId);
        Task<PaginatedList<WalletTransaction>> GetByWalletIdAsync(Guid walletId, DateTime from, DateTime to,PageRequest pageRequest);
    }

}
