using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ICartItemRepository
    {
        Task<CartItem?> GetByIdAsync(Guid itemId);
        Task<List<CartItem>> GetByCartIdAsync(Guid cartId);
        Task AddAsync(CartItem item);
        Task UpdateAsync(CartItem item);
        Task DeleteAsync(CartItem item);
        Task<PaginatedList<CartItem>> GetPaginatedAsync(Guid cartId, PageRequest pageRequest);
        Task<decimal> GetItemTotalAsync(Guid itemId);
    }

}
