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
    public interface IDeliveryAgentRepository : IBaseRepository<DeliveryAgent>
    {
        Task<DeliveryAgent?> GetByEmailAsync(string email);
        Task UpdateDeliveryAgentAsync(DeliveryAgent deliveryAgent);
        Task DeleteDeliveryAgentAsync(Guid deliveryAgentId);
        Task<PaginatedList<DeliveryAgent>> GetDeliveryAgentsByStatusAsync(string status, PageRequest pageRequest);
        Task<PaginatedList<DeliveryAgent>> SearchDeliveryAgentsAsync(string keyword, PageRequest pageRequest);
    }
}
