using Application.Dto;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repository
{
    public interface IStockMovementRepository : IBaseRepository<StockMovement>
    {
        Task<IEnumerable<StockMovement>> GetByWarehouseItemIdAsync(Guid warehouseItemId);
        Task<PaginatedList<StockMovementDto>> GetByProductId(Guid productId,PageRequest pageRequest);
        Task<PaginatedList<StockMovementDto>> GetByMovementType(StockMovementType movementType, PageRequest pageRequest);
        Task<PaginatedList<StockMovementDto>> GetByDateRangeAsync(DateTime start, DateTime end,PageRequest pageRequest);
        Task<int> GetTotalQuantityInAsync(Guid warehouseItemId);
        Task<int> GetTotalQuantityOutAsync(Guid warehouseItemId);
        Task<PaginatedList<StockMovementDto>> GetByWarehousePagedAsync(Guid warehouseId, PageRequest pageRequest);
        Task<StockMovementStatsDto> GetStockMovementStatsAsync();
        Task<List<StockMovementTrendDto>> GetStockMovementTrendAsync(string range);
    }

}
