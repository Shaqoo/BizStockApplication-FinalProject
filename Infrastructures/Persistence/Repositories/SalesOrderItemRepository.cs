using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class SalesOrderItemRepository : ISalesOrderItemRepository
    {
        private readonly BizStockContext _context;

        public SalesOrderItemRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SalesOrderItem item)
        {
            await _context.SalesOrderItems.AddAsync(item);
        }

        public async Task<SalesOrderItem?> GetByIdAsync(Guid id)
        {
            return await _context.SalesOrderItems.FindAsync(id)
                ?? throw new EntityNotFoundException("Sales order","Id");
        }

        public async Task<PaginatedList<SalesOrderItem>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.SalesOrderItems.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<SalesOrderItem>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<SalesOrderItem>> FindAsync(Expression<Func<SalesOrderItem, bool>> predicate)
        {
            return await _context.SalesOrderItems.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<SalesOrderItem>> GetBySalesOrderIdAsync(Guid salesOrderId)
        {
            return await _context.SalesOrderItems
                .Where(i => i.SalesOrderId == salesOrderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<SalesOrderItem>> GetByProductIdAsync(Guid productId)
        {
            return await _context.SalesOrderItems
                .Where(i => i.ProductId == productId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalSalesForProductAsync(Guid productId)
        {
            return await _context.SalesOrderItems
                .Where(i => i.ProductId == productId)
                .SumAsync(i => i.TotalPrice);
        }

        public async Task<int> GetTotalUnitsSoldForProductAsync(Guid productId)
        {
            return await _context.SalesOrderItems
                .Where(i => i.ProductId == productId)
                .SumAsync(i => i.Quantity);
        }

        public async Task<PaginatedList<SalesOrderItem>> GetByDateRangeAsync(DateTime start, DateTime end, PageRequest pageRequest)
        {
            var query = _context.SalesOrderItems
                .Include(i => i.SalesOrder)
                .Where(i => i.SalesOrder.DateCreated >= start && i.SalesOrder.DateCreated <= end);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.SalesOrder.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<SalesOrderItem>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<SalesOrderItem> GetByExpression(Expression<Func<SalesOrderItem, bool>> predicate)
        {
            return await _context.SalesOrderItems.FirstOrDefaultAsync(predicate) ??
                throw new EntityNotFoundException("Sales Order Item","Predicate");
        }
    }

}
