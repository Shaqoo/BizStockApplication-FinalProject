using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
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
    public class DeliveryFeeRuleRepository : IDeliveryFeeRuleRepository
    {
        private readonly BizStockContext _context;

        public DeliveryFeeRuleRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DeliveryFeeRule rule)
        {
            await _context.DeliveryFeeRules.AddAsync(rule);
        }

        public async Task<DeliveryFeeRule?> GetByIdAsync(Guid id)
        {
            return await _context.DeliveryFeeRules.FindAsync(id)
                ?? throw new KeyNotFoundException("Delivery fee rule not found.");
        }

        public async Task<PaginatedList<DeliveryFeeRule>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.DeliveryFeeRules.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<DeliveryFeeRule>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<DeliveryFeeRule>> FindAsync(Expression<Func<DeliveryFeeRule, bool>> predicate)
        {
            return await _context.DeliveryFeeRules.Where(predicate).ToListAsync();
        }

        public async Task<DeliveryFeeRule?> GetByZoneAsync(string zone)
        {
            return await _context.DeliveryFeeRules
                .FirstOrDefaultAsync(r => r.Zone.ToLower() == zone.ToLower());
        }

        public async Task<DeliveryFeeRule?> GetActiveRuleByZoneAsync(string zone)
        {
            return await _context.DeliveryFeeRules
                .FirstOrDefaultAsync(r => r.Zone.ToLower() == zone.ToLower() && r.IsActive);
        }

        public async Task<decimal> CalculateDeliveryFeeAsync(string zone, decimal orderAmount)
        {
            var rule = await GetActiveRuleByZoneAsync(zone);

            if (rule == null)
                throw new InvalidOperationException("No active delivery fee rule found for the specified zone.");

            if (rule.MinOrderAmountForFree.HasValue && orderAmount >= rule.MinOrderAmountForFree.Value)
                return 0m;

            return rule.FlatRate;
        }

        public async Task<bool> IsFreeDeliveryAsync(string zone, decimal orderAmount)
        {
            var rule = await GetActiveRuleByZoneAsync(zone);

            if (rule == null) return false;

            return rule.MinOrderAmountForFree.HasValue && orderAmount >= rule.MinOrderAmountForFree.Value;
        }

        public async Task UpdateDeliveryFeeRule(DeliveryFeeRule deliveryFeeRule)
        {
            _context.DeliveryFeeRules.Update(deliveryFeeRule);
            await Task.CompletedTask;
        }

        public async Task<bool> DeleteRule(Guid ruleId)
        {
            var rule = await _context.DeliveryFeeRules.FindAsync(ruleId);

            if (rule == null)
                return false;

            _context.DeliveryFeeRules.Remove(rule);
            return true;
        }

        public async Task<DeliveryFeeRule> GetByExpression(Expression<Func<DeliveryFeeRule, bool>> predicate)
        {
            return await _context.DeliveryFeeRules.FirstOrDefaultAsync(predicate)
                ?? throw new ArgumentNullException("Delivery Fee Rule Not Found");
        }
    }

}
