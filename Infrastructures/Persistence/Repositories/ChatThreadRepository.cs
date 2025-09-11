using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class ChatThreadRepository : IChatThreadRepository
    {
        private readonly BizStockContext _context;

        public ChatThreadRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChatThread chatThread)
        {
            await _context.ChatThreads.AddAsync(chatThread);
        }

        public async Task<ChatThread?> GetByIdAsync(Guid id)
        {
            return await _context.ChatThreads.FirstOrDefaultAsync(a => a.Id == id) ?? null;
        }

        public async Task<PaginatedList<ChatThread>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.ChatThreads.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.DateCreated) 
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<ChatThread>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<ChatThread>> FindAsync(Expression<Func<ChatThread, bool>> predicate)
        {
            return await _context.ChatThreads.Where(predicate).ToListAsync();
        }

        public async Task<PaginatedList<ChatThread>> GetByCustomerIdAsync(Guid customerId,PageRequest pageRequest)
        {
            var query = _context.ChatThreads.Where(a => a.CustomerId == customerId).AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<ChatThread>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<ChatThread>> GetByAgentIdAsync(Guid agentId, PageRequest pageRequest)
        {
            var query = _context.ChatThreads.Where(a => a.AssignedAgentId == agentId && a.Status == ChatStatus.InProgress).AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<ChatThread>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<ChatThread?> GetThreadWithMessagesAsync(Guid threadId)
        {
            return await _context.ChatThreads
                .Include(t => t.Messages.OrderBy(m => m.SentAt))
                .FirstOrDefaultAsync(t => t.Id == threadId);
        }

        public async Task<int> CountOpenThreadsAsync()
        {
            return await _context.ChatThreads
                .CountAsync(t => t.Status == ChatStatus.Open);
        }

        public async Task<int> CountThreadsByAgentAsync(Guid agentId)
        {
            return await _context.ChatThreads
                .CountAsync(t => t.AssignedAgentId == agentId);
        }

        public async Task<IEnumerable<ChatThread>> GetOpenThreadsWithoutAgentAsync()
        {
            return await _context.ChatThreads
                .Where(t => t.Status == ChatStatus.Open && t.AssignedAgentId == null)
                .OrderBy(t => t.Id)
                .ToListAsync();
        }

        public async Task UpdateThread(ChatThread chatThread)
        {
            _context.ChatThreads.Update(chatThread);
            await Task.CompletedTask; 
        }

        public async Task<ChatThread?> GetByExpression(Expression<Func<ChatThread, bool>> predicate)
        {
            return await _context.ChatThreads.FirstOrDefaultAsync(predicate);
        }

        public async Task<PaginatedList<ChatThread>> GetByStatusAsync(ChatStatus status, PageRequest pageRequest)
        {
            var query = _context.ChatThreads.Where(a => a.Status == status).AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<ChatThread>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<int> CountClosedThreadsAsync()
        {
            return await _context.ChatThreads
                .CountAsync(t => t.Status == ChatStatus.Closed);
        }

        public async Task<int> CountInprogressThreadsAsync()
        {
            return await _context.ChatThreads
                .CountAsync(t => t.Status == ChatStatus.InProgress);
        }
    }

}
