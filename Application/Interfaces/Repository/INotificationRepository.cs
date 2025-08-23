using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetByRecipientAsync(Guid recipientId);
        Task<IEnumerable<Notification>> GetUnreadByRecipientAsync(Guid recipientId);
        Task<int> CountUnreadByRecipientAsync(Guid recipientId);
        Task MarkAsReadAsync(Guid notificationId);
        Task MarkAllAsReadAsync(Guid recipientId);
        Task<PaginatedList<Notification>> GetByRecipientPagedAsync(Guid recipientId, PageRequest pageRequest);
        Task UpdateAsync(Notification notification);
        Task DeleteAsync(Guid notificationId);
    }

}
