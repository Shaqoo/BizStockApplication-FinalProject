using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IChatMessageRepository : IBaseRepository<ChatMessage>
    {
        Task<IEnumerable<ChatMessage>> GetMessagesByThreadIdAsync(Guid threadId);
        Task<PaginatedList<ChatMessage>> GetMessagesByThreadIdPagedAsync(Guid threadId, PageRequest pageRequest);
        Task<ChatMessage?> GetMessageWithReplyAsync(Guid messageId);
        Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(Guid threadId, Guid userId);
        Task<int> CountUnreadMessagesAsync(Guid threadId, Guid userId);
        Task MarkMessagesAsReadAsync(Guid threadId, Guid userId);
        Task UpdateMessage(ChatMessage message);
    }


}
