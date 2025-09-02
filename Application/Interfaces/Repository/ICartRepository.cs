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
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Cart cart);
        Task<decimal> GetTotalCountAsync(Guid cartId);
        Task<decimal> GetTotalPriceAsync(Guid cartId);
        Task<PaginatedList<CartItem>> GetCartItemsAsync(Guid cartId, PageRequest pageRequest);
    }


}
