using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IWalletRepository : IBaseRepository<Wallet>
    {
        Task<Wallet?> GetByUserIdAsync(Guid userId);
        Task<bool> VerifyPinAsync(Guid userId, string rawPin);
        Task SetPinAsync(Guid userId, string rawPin);
        Task<decimal> GetBalanceAsync(Guid userId);
        Task<bool> HasSufficientBalanceAsync(Guid userId, decimal amount);
        Task UpdateAsync(Wallet wallet);
    }

}
