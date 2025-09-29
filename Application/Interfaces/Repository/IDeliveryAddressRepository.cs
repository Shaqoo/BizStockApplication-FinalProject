using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IDeliveryAddressRepository
    {
        Task<DeliveryAddress?> GetByIdAsync(Guid id);
        Task<IEnumerable<DeliveryAddress>> GetByCustomerIdAsync(Guid customerId);
        Task AddAsync(DeliveryAddress address);
        Task UpdateAsync(DeliveryAddress address);
        Task DeleteAsync(Guid id);
        Task<DeliveryAddress?> GetDefaultAsync(Guid customerId);
    }
}
