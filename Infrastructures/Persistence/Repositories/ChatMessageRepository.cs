using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly BizStockContext _context;

        public ChatMessageRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
        }

        public async Task<ChatMessage?> GetByIdAsync(Guid id)
        {
            return await _context.ChatMessages.Include(a => a.Reactions).FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new KeyNotFoundException("Message not found.");
        }

        public async Task<PaginatedList<ChatMessage>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.ChatMessages.Include(a => a.Reactions).AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(m => m.SentAt)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<ChatMessage>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<ChatMessage>> FindAsync(Expression<Func<ChatMessage, bool>> predicate)
        {
            return await _context.ChatMessages.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByThreadIdAsync(Guid threadId)
        {
            return await _context.ChatMessages.Include(a => a.Reactions)
                .Where(m => m.ChatThreadId == threadId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<PaginatedList<ChatMessage>> GetMessagesByThreadIdPagedAsync(Guid threadId, PageRequest pageRequest)
        {
            var query = _context.ChatMessages.Include(a => a.Reactions)
                .Where(m => m.ChatThreadId == threadId)
                .OrderBy(m => m.SentAt);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<ChatMessage>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<ChatMessage?> GetMessageWithReplyAsync(Guid messageId)
        {
            return await _context.ChatMessages
                .Include(m => m.RepliedToMessage)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(Guid threadId, Guid userId)
        {
            return await _context.ChatMessages
                .Where(m => m.ChatThreadId == threadId && m.SenderId != userId && !m.IsRead)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<int> CountUnreadMessagesAsync(Guid threadId, Guid userId)
        {
            return await _context.ChatMessages
                .CountAsync(m => m.ChatThreadId == threadId && m.SenderId != userId && !m.IsRead);
        }

        public async Task MarkMessagesAsReadAsync(Guid threadId, Guid userId)
        {
            var unreadMessages = await _context.ChatMessages
                .Where(m => m.ChatThreadId == threadId && m.SenderId != userId && !m.IsRead)
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.MarkAsRead();
            }
        }

        public async Task<ChatMessage> GetByExpression(Expression<Func<ChatMessage, bool>> predicate)
        {
            return await _context.ChatMessages.FirstOrDefaultAsync(predicate) ?? 
                  throw new ArgumentNullException("Message Not Found");
        }

        public Task UpdateMessage(ChatMessage message)
        {
             _context.ChatMessages.Update(message);
                return Task.CompletedTask;
        }
    }

}
