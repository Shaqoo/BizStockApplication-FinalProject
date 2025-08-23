using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IChatThreadRepository : IBaseRepository<ChatThread>
    {
        Task<PaginatedList<ChatThread>> GetByCustomerIdAsync(Guid customerId, PageRequest pageRequest);
        Task<PaginatedList<ChatThread>> GetByAgentIdAsync(Guid agentId, PageRequest pageRequest);
        Task<ChatThread?> GetThreadWithMessagesAsync(Guid threadId);
        Task<PaginatedList<ChatThread>> GetByStatusAsync(ChatStatus status, PageRequest pageRequest);
        Task<int> CountOpenThreadsAsync();
        Task<int> CountThreadsByAgentAsync(Guid agentId);
        Task<IEnumerable<ChatThread>> GetOpenThreadsWithoutAgentAsync();
        Task UpdateThread(ChatThread chatThread);
    }

}
