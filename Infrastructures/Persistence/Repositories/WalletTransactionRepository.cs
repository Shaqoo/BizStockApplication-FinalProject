using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly BizStockContext _context;

        public WalletTransactionRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WalletTransaction entity)
        {
            await _context.WalletTransactions.AddAsync(entity);
        }

        public async Task<WalletTransaction?> GetByIdAsync(Guid id)
        {
            return await _context.WalletTransactions.FindAsync(id)
                ?? throw new EntityNotFoundException("Wallet transaction","Id");
        }

        public async Task<PaginatedList<WalletTransaction>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.WalletTransactions.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<WalletTransaction>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<WalletTransaction>> FindAsync(Expression<Func<WalletTransaction, bool>> predicate)
        {
            return await _context.WalletTransactions
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(Guid walletId)
        {
            return await _context.WalletTransactions
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<PaginatedList<WalletTransaction>> GetByWalletPagedAsync(Guid walletId, PageRequest pageRequest)
        {
            var query = _context.WalletTransactions
                .Where(t => t.WalletId == walletId);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<WalletTransaction>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<WalletTransaction>> GetByUserIdAsync(Guid userId)
        {
            return await _context.WalletTransactions
                .Where(t => t.Wallet.UserId == userId)
                .OrderByDescending(t => t.Id)
                .ToListAsync();
        }

        public async Task<WalletTransaction?> GetByReferenceAsync(string reference)
        {
            return await _context.WalletTransactions
                .FirstOrDefaultAsync(t => t.Reference == reference);
        }

        public async Task<decimal> GetTotalCreditsAsync(Guid walletId)
        {
            return await _context.WalletTransactions
                .Where(t => t.WalletId == walletId && t.Type == TransactionType.Credit)
                .SumAsync(t => t.Amount);
        }

        public async Task<decimal> GetTotalDebitsAsync(Guid walletId)
        {
            return await _context.WalletTransactions
                .Where(t => t.WalletId == walletId && t.Type == TransactionType.Debit)
                .SumAsync(t => t.Amount);
        }

        public async Task<PaginatedList<WalletTransaction>> GetByWalletIdAsync(Guid walletId, DateTime from, DateTime to, PageRequest pageRequest)
        {
            var query = _context.WalletTransactions
                .Where(t => t.WalletId == walletId && t.DateCreated >= from && t.DateCreated <= to);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<WalletTransaction>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<WalletTransaction> GetByExpression(Expression<Func<WalletTransaction, bool>> predicate)
        {
            return await _context.WalletTransactions.FirstOrDefaultAsync(predicate) ??
                throw new EntityNotFoundException("Transaction","Predicate"); 
        }
    }

}
