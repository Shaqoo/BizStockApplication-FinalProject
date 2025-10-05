using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class DeliveryAssignmentRepository : IDeliveryAssignmentRepository
    {
        private readonly BizStockContext _context;

        public DeliveryAssignmentRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DeliveryAssignment deliveryAssignment)
        {
            await _context.DeliveryAssignments.AddAsync(deliveryAssignment);
        }

        public async Task<DeliveryAssignment?> GetByIdAsync(Guid id)
        {
            return await _context.DeliveryAssignments
                .Include(a => a.DeliveryAgent)
                .FirstOrDefaultAsync(a => a.Id.Equals(id));
        }

        public async Task<PaginatedList<DeliveryAssignment>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.DeliveryAssignments
                .Include(a => a.DeliveryAgent)
                .AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(d => d.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<DeliveryAssignment>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<DeliveryAssignment>> FindAsync(Expression<Func<DeliveryAssignment, bool>> predicate)
        {
            return await _context.DeliveryAssignments.Where(predicate).ToListAsync();
        }

        public async Task<DeliveryAssignment?> GetBySalesOrderIdAsync(Guid salesOrderId)
        {
            return await _context.DeliveryAssignments
                .Include(a => a.DeliveryAgent)
                .FirstOrDefaultAsync(d => d.SalesOrderId == salesOrderId);
        }

        public async Task UpdateDeliveryAssignment(DeliveryAssignment deliveryAssignment)
        {
            _context.DeliveryAssignments.Update(deliveryAssignment);
            await Task.CompletedTask;
        }

        public async Task<PaginatedList<DeliveryAssignment>> GetByDeliveryAgentIdAsync(Guid deliveryAgentId, PageRequest pageRequest)
        {
            var query = _context.DeliveryAssignments
                .Where(d => d.DeliveryAgentId == deliveryAgentId);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(d => d.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<DeliveryAssignment>(items, total, 1, total);  
        }

        public async Task<IEnumerable<DeliveryAssignment>> GetPendingDeliveriesAsync(Guid deliveryAgentId)
        {
            return await _context.DeliveryAssignments
                .Where(d => d.DeliveryAgentId == deliveryAgentId && d.Status == DeliveryStatus.Pending)
                .OrderBy(d => d.Id)
                .ToListAsync();
        }

        public async Task<PaginatedList<DeliveryAssignment>> GetDeliveredOrdersAsync(DateTime startDate, DateTime endDate, PageRequest pageRequest)
        {
            var query = _context.DeliveryAssignments
                .Where(d => d.Status == DeliveryStatus.Delivered && d.DeliveredAt >= startDate && d.DeliveredAt <= endDate);

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(d => d.DeliveredAt)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<DeliveryAssignment>(items, total, 1, total); 
        }

        public async Task<decimal> GetTotalDeliveryFeesAsync(Guid deliveryAgentId, DateTime startDate, DateTime endDate)
        {
            return await _context.DeliveryAssignments
                .Where(d =>
                    d.DeliveryAgentId == deliveryAgentId &&
                    d.Status == DeliveryStatus.Delivered &&
                    d.DeliveredAt >= startDate && d.DeliveredAt <= endDate)
                .SumAsync(d => d.DeliveryFee);
        }

        public async Task<int> CountPendingDeliveriesAsync(Guid deliveryAgentId)
        {
            return await _context.DeliveryAssignments
                .CountAsync(d => d.DeliveryAgentId == deliveryAgentId && d.Status == DeliveryStatus.Pending);
        }

        public async Task<int> CountDeliveredOrdersAsync(Guid deliveryAgentId)
        {
            return await _context.DeliveryAssignments
                .CountAsync(d => d.DeliveryAgentId == deliveryAgentId && d.Status == DeliveryStatus.Delivered);
        }

        public async Task<DeliveryAssignment?> GetByExpression(Expression<Func<DeliveryAssignment, bool>> predicate)
        {
            return await _context.DeliveryAssignments.FirstOrDefaultAsync(predicate) ??
                throw new ArgumentNullException("Delivery Assignment Not Found");
        }
    }

}
