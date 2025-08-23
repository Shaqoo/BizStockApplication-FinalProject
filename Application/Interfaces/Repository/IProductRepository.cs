using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<bool> Exists(Guid Id);
        Task UpdateAsync(Product product);
        Task<PaginatedList<Product>> GetProductsByCategoryId(Guid categoryId, PageRequest pageRequest);
        Task<PaginatedList<Product>> GetProductsOrderedByPrice(PageRequest pageRequest, bool ascending = true);
        Task<PaginatedList<Product>> GetProductsByCategoryOrderedByPrice(Guid categoryId, PageRequest pageRequest, bool ascending = true);
        Task<PaginatedList<Product>> SearchProductsAsync(string keyword, PageRequest pageRequest);
        Task<PaginatedList<Product>> GetProductsByWarehouseIdAsync(Guid warehouseId, PageRequest pageRequest);
        Task<PaginatedList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice, PageRequest pageRequest);
        Task<PaginatedList<Product>> GetProductsWithLowStockAsync(PageRequest pageRequest);
        Task<PaginatedList<Product>> GetRecentlyAddedProductsAsync(PageRequest pageRequest);
        Task<PaginatedList<Product>> GetTopRatedProductsAsync(PageRequest pageRequest);
        Task<PaginatedList<Product>> GetProductsByStatus(PageRequest pageRequest,ProductStatus productStatus);
    }

}
