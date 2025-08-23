using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IMessageReactionRepository : IBaseRepository<MessageReaction>
    {
        Task<IEnumerable<MessageReaction>> GetReactionsByMessageIdAsync(Guid messageId);
        Task<IEnumerable<MessageReaction>> GetReactionsByUserAsync(Guid userId);
        Task<MessageReaction?> GetUserReactionForMessageAsync(Guid messageId, Guid userId);
        Task<bool> HasUserReactedWithEmojiAsync(Guid messageId, Guid userId, string emoji);
        Task UpdateAsync(MessageReaction reaction);
        Task DeleteAsync(Guid reactionId);
    }

}
