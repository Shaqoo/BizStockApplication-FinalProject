using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repository
{
    public interface IChatThreadRepository : IBaseRepository<ChatThread>
    {
        Task<PaginatedList<ChatThread>> GetByCustomerIdAsync(Guid customerId, PageRequest pageRequest);
        Task<PaginatedList<ChatThread>> GetByAgentIdAsync(Guid agentId, PageRequest pageRequest);
        Task<ChatThread?> GetThreadWithMessagesAsync(Guid threadId);
        Task<PaginatedList<ChatThread>> GetByStatusAsync(ChatStatus status, PageRequest pageRequest);
        Task<int> CountOpenThreadsAsync();
        Task<int> CountClosedThreadsAsync();
        Task<int> CountInprogressThreadsAsync();
        Task<int> CountThreadsByAgentAsync(Guid agentId);
        Task<IEnumerable<ChatThread>> GetOpenThreadsWithoutAgentAsync();
        Task UpdateThread(ChatThread chatThread);
    }

}
