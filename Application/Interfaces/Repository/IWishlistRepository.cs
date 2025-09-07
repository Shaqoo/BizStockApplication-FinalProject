using Application.Dto;
using Application.Pagination;
using Domain.Entities;
using Domain.Entities.Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetByIdAsync(Guid wishlistId);
        Task<Wishlist?> GetByUserIdAsync(Guid userId);
        Task<bool> CheckIfItemExists(Guid userId, Guid productId);
        Task AddItemsAsync(WishlistItem wishlist);
        Task AddAsync(Wishlist wishlist);
        Task AddRangeAsync(IEnumerable<WishlistItem> wishlistItems);
        Task UpdateAsync(Wishlist wishlist);
        Task DeleteAsync(Wishlist wishlist);
        Task<PaginatedList<WishlistItemDto>> GetAllByUserAsync(PageRequest pageRequest,Guid userId);
    }

}
