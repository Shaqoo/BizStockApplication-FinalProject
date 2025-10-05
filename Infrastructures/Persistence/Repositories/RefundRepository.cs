using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructures.Persistence.Repositories
{
    public class RefundRepository : IRefundRepository
    {
        private readonly BizStockContext _context;

        public RefundRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<Refund?> GetByIdAsync(Guid id)
        {
            return await _context.Refunds
                .Include(r => r.Order)
                .ThenInclude(r => r.Customer)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Refund>> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Refunds
                .Include(r => r.Order)
                .Where(r => r.SalesOrderId == orderId)
                .ToListAsync();
        }

        public async Task AddAsync(Refund refund)
        {
            await _context.Refunds.AddAsync(refund);
        }

        public async Task UpdateAsync(Refund refund)
        {
            _context.Refunds.Update(refund);
            await Task.CompletedTask;
        }

        public async Task<PaginatedList<Refund>> GetAllAsync(PageRequest pageRequest)
        {
            var query =  _context.Refunds
                .Include(r => r.Order);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Refund>(items, total, pageRequest.Page, pageRequest.PageSize);
        }
    }
}
