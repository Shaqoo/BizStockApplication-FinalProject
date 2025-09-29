using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly BizStockContext _context;

        public WalletRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
        }

        public async Task<Wallet?> GetByIdAsync(Guid id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginatedList<Wallet>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Wallets.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Wallet>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Wallet>> FindAsync(Expression<Func<Wallet, bool>> predicate)
        {
            return await _context.Wallets
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Wallet?> GetByUserIdAsync(Guid customerId)
        {
            return await _context.Wallets
                .FirstOrDefaultAsync(w => w.CustomerId == customerId);
        }

        public async Task<bool> VerifyPinAsync(Guid customerId, string rawPin)
        {
            var user = await _context.Wallets
                .Where(u => u.CustomerId == customerId)
                .Select(u => new { u.PinHash })
                .FirstOrDefaultAsync();

            if (user == null || string.IsNullOrEmpty(user.PinHash))
                return false;

            return rawPin == user.PinHash;
        }

        public async Task SetPinAsync(Guid customerId, string rawPin)
        {
            var user = await _context.Wallets.FirstOrDefaultAsync(a => a.CustomerId == customerId);
            if (user == null) throw new EntityNotFoundException("Wallet","UserId");

             user.SetPin(rawPin);  
            _context.Wallets.Update(user);
            await Task.CompletedTask;
        }

        public async Task<decimal> GetBalanceAsync(Guid customerId)
        {
            var wallet = await _context.Wallets
                .Where(w => w.CustomerId == customerId)
                .Select(w => w.Balance)
                .FirstOrDefaultAsync();

            return wallet;
        }

        public async Task<bool> HasSufficientBalanceAsync(Guid customerId, decimal amount)
        {
            var balance = await GetBalanceAsync(customerId);
            return balance >= amount;
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await Task.CompletedTask;
        }

        public async Task<Wallet?> GetByExpression(Expression<Func<Wallet, bool>> predicate)
        {
             return await _context.Wallets.FirstOrDefaultAsync(predicate) ??
                throw new EntityNotFoundException("Wallet","Predicate");
        }
    }

}
