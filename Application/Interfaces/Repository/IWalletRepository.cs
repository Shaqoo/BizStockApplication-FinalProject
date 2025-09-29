using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IWalletRepository : IBaseRepository<Wallet>
    {
        Task<Wallet?> GetByUserIdAsync(Guid customerId);
        Task<bool> VerifyPinAsync(Guid customerId, string rawPin);
        Task SetPinAsync(Guid customerId, string rawPin);
        Task<decimal> GetBalanceAsync(Guid customerId);
        Task<bool> HasSufficientBalanceAsync(Guid customerId, decimal amount);
        Task UpdateAsync(Wallet wallet);
    }

}
