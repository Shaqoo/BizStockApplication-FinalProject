using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IWalletTransactionRepository : IBaseRepository<WalletTransaction>
    {
        Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId);
        Task<IEnumerable<WalletTransaction>> GetByUserIdAsync(Guid userId); 
        Task<WalletTransaction?> GetByReferenceAsync(string reference);
        Task<PaginatedList<WalletTransaction>> GetByWalletPagedAsync(Guid walletId, PageRequest pageRequest);
        Task<decimal> GetTotalCreditsAsync(Guid walletId);
        Task<decimal> GetTotalDebitsAsync(Guid walletId);
        Task<PaginatedList<WalletTransaction>> GetByWalletIdAsync(Guid walletId, DateTime from, DateTime to,PageRequest pageRequest);
    }

}
