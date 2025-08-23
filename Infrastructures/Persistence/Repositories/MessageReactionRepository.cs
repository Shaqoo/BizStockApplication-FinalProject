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
    public class MessageReactionRepository : IMessageReactionRepository
    {
        private readonly BizStockContext _context;

        public MessageReactionRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(MessageReaction reaction)
        {
            await _context.MessageReactions.AddAsync(reaction);
        }

        public async Task<MessageReaction?> GetByIdAsync(Guid id)
        {
            return await _context.MessageReactions.FindAsync(id)
                ?? throw new KeyNotFoundException("Reaction not found.");
        }

        public async Task<PaginatedList<MessageReaction>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.MessageReactions
                .Include(r => r.Message)
                .Include(r => r.ReactedBy)
                .OrderByDescending(r => r.ReactedAt);

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<MessageReaction>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<MessageReaction>> FindAsync(Expression<Func<MessageReaction, bool>> predicate)
        {
            return await _context.MessageReactions
                .Include(r => r.Message)
                .Include(r => r.ReactedBy)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MessageReaction>> GetReactionsByMessageIdAsync(Guid messageId)
        {
            return await _context.MessageReactions
                .Where(r => r.MessageId == messageId)
                .ToListAsync();
        }

        public async Task<IEnumerable<MessageReaction>> GetReactionsByUserAsync(Guid userId)
        {
            return await _context.MessageReactions
                .Where(r => r.ReactedByUserId == userId)
                .ToListAsync();
        }

        public async Task<MessageReaction?> GetUserReactionForMessageAsync(Guid messageId, Guid userId)
        {
            return await _context.MessageReactions
                .FirstOrDefaultAsync(r => r.MessageId == messageId && r.ReactedByUserId == userId);
        }

        public async Task<bool> HasUserReactedWithEmojiAsync(Guid messageId, Guid userId, string emoji)
        {
            return await _context.MessageReactions
                .AnyAsync(r => r.MessageId == messageId && r.ReactedByUserId == userId && r.Emoji == emoji);
        }

        public async Task UpdateAsync(MessageReaction reaction)
        {
            _context.MessageReactions.Update(reaction);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid reactionId)
        {
            var reaction = await _context.MessageReactions.FindAsync(reactionId);
            if (reaction != null)
            {
                _context.MessageReactions.Remove(reaction);
            }

            await Task.CompletedTask;
        }

        public async Task<MessageReaction> GetByExpression(Expression<Func<MessageReaction, bool>> predicate)
        {
            return await _context.MessageReactions.FirstOrDefaultAsync(predicate) ??
               throw new ArgumentNullException("Reaction Not Found");
        }
    }

}
