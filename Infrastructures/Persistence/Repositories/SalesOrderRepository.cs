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
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly BizStockContext _context;

        public SalesOrderRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SalesOrder salesOrder)
        {
            await _context.SalesOrders.AddAsync(salesOrder);
        }

        public async Task<SalesOrder?> GetByIdAsync(Guid id)
        {
            return await _context.SalesOrders.FindAsync(id)
                ?? throw new EntityNotFoundException("Sales order","Id");
        }

        public async Task<PaginatedList<SalesOrder>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.SalesOrders.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<SalesOrder>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<SalesOrder>> FindAsync(Expression<Func<SalesOrder, bool>> predicate)
        {
            return await _context.SalesOrders.Where(predicate).ToListAsync();
        }

        public async Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.SalesOrders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<PaginatedList<SalesOrder>> GetByCustomerIdAsync(Guid customerId, PageRequest pageRequest)
        {
            var query = _context.SalesOrders.Where(o => o.CustomerId == customerId);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<SalesOrder>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<SalesOrder>> GetByStatusAsync(OrderStatus status, PageRequest pageRequest)
        {
            var query = _context.SalesOrders.Where(o => o.Status == status);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<SalesOrder>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<SalesOrder?> GetWithItemsAsync(Guid salesOrderId)
        {
            return await _context.SalesOrders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == salesOrderId);
        }

        public async Task<SalesOrder?> GetWithDeliveryAssignmentAsync(Guid salesOrderId)
        {
            return await _context.SalesOrders
                .Include(o => o.DeliveryAssignment)
                .FirstOrDefaultAsync(o => o.Id == salesOrderId);
        }

        public async Task<decimal> GetTotalSalesAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.SalesOrders
                .Where(o => o.DateCreated >= startDate && o.DateCreated <= endDate && o.Status == OrderStatus.Delivered)
                .SumAsync(o => o.Total);
        }

        public async Task<int> CountByStatusAsync(OrderStatus status)
        {
            return await _context.SalesOrders.CountAsync(o => o.Status == status);
        }

        public async Task UpdateStatusAsync(SalesOrder salesOrder)
        {
            _context.SalesOrders.Update(salesOrder);
            await Task.CompletedTask;
        }

        public async Task<SalesOrder> GetByExpression(Expression<Func<SalesOrder, bool>> predicate)
        {
            return await _context.SalesOrders.FirstOrDefaultAsync(predicate) ??
               throw new EntityNotFoundException("Sales Order","Predicate");
        }
    }

}
