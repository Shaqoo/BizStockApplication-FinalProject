using Application.Dto;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(Guid cartId);
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task<Cart?> GetBySessionIdAsync(string sessionId);
        Task<PaginatedList<CartItemDto>> GetCartItemsAsync(Guid cartId, PageRequest pageRequest);
        Task<PaginatedList<CartItemDto>> GetCartItemsByUserIdAsync(Guid userId, PageRequest pageRequest);
        Task<PaginatedList<CartItemDto>> GetCartItemsBySessionIdAsync(string sessionId, PageRequest pageRequest);
        Task AddAsync(Cart cart);
        Task AddRangeAsync(IEnumerable<CartItem> cartItemss);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Cart cart);
    }

}
