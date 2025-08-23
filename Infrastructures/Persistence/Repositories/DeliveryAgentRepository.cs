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
    public class DeliveryAgentRepository : IDeliveryAgentRepository
    {
        private readonly BizStockContext _context;

        public DeliveryAgentRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DeliveryAgent deliveryAgent)
        {
            await _context.DeliveryAgents.AddAsync(deliveryAgent);
        }

        public async Task<DeliveryAgent?> GetByIdAsync(Guid id)
        {
            return await _context.DeliveryAgents.FindAsync(id)
                ?? throw new KeyNotFoundException("Delivery agent not found.");
        }

        public async Task<PaginatedList<DeliveryAgent>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.DeliveryAgents.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.FullName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<DeliveryAgent>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<DeliveryAgent>> FindAsync(Expression<Func<DeliveryAgent, bool>> predicate)
        {
            return await _context.DeliveryAgents.Where(predicate).ToListAsync();
        }

        public async Task<DeliveryAgent> GetByEmailAsync(string email)
        {
            return await _context.DeliveryAgents
                .FirstOrDefaultAsync(a => a.Email.Value.ToLower() == email.ToLower())
                ?? throw new KeyNotFoundException("Delivery agent not found by email.");
        }

        public async Task UpdateDeliveryAgentAsync(DeliveryAgent deliveryAgent)
        {
            _context.DeliveryAgents.Update(deliveryAgent);
            await Task.CompletedTask;
        }

        public async Task DeleteDeliveryAgentAsync(Guid deliveryAgentId)
        {
            var agent = await _context.DeliveryAgents.FindAsync(deliveryAgentId);

            if (agent == null)
                throw new KeyNotFoundException("Delivery agent not found.");

            _context.DeliveryAgents.Remove(agent);
            await Task.CompletedTask;
        }

        public async Task<PaginatedList<DeliveryAgent>> GetDeliveryAgentsByStatusAsync(string status, PageRequest pageRequest)
        {
            var query = _context.DeliveryAgents
                .Where(a => a.AvailabilityStatus.ToString().ToLower() == status.ToLower());

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.FullName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<DeliveryAgent>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<DeliveryAgent>> SearchDeliveryAgentsAsync(string keyword, PageRequest pageRequest)
        {
            var formattedKeyword = keyword.Trim().Replace(" ", " & ");

            var query = _context.DeliveryAgents
                .Where(a =>
                    EF.Functions.ToTsVector("english", EF.Property<string>(a, "SearchVector"))
                        .Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword)));

            var totalCount = await query.CountAsync();

       
            if (totalCount == 0)
            {
                query = _context.DeliveryAgents
                    .Where(a => EF.Functions.TrigramsSimilarity(a.FullName, keyword) > 0.3)
                    .OrderByDescending(a => EF.Functions.TrigramsSimilarity(a.FullName, keyword));

                totalCount = await query.CountAsync();
            }

            var items = await query
                .OrderBy(a => a.FullName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<DeliveryAgent>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<DeliveryAgent> GetByExpression(Expression<Func<DeliveryAgent, bool>> predicate)
        {
            return await _context.DeliveryAgents.FirstOrDefaultAsync(predicate) ??
                throw new ArgumentNullException("Delivery Agent Not Found");
        }
    }

}
