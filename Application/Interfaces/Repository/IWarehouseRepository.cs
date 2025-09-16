using Application.Dto;
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
    public interface IWarehouseRepository : IBaseRepository<Warehouse>
    {
        Task<Warehouse> GetByLocationAsync(string location);
        Task<int> GetCount(Guid warehouseId);
        Task<bool> HasItemAsync(Guid warehouseId);
        Task<bool> Exists(Guid Id);
        Task UpdateWarehouseAsync(Warehouse warehouse);
        Task DeleteWarehouseAsync(Guid warehouseId);
        Task<bool> IsNameUnique(string name);
        Task<PaginatedList<WarehouseDto>> GetAllAsyncWithDto(PageRequest pageRequest);
        Task<PaginatedList<WarehouseDto>> SearchWarehousesAsync(string keyword,PageRequest pageRequest);
        Task<List<WarehouseStockDto>> GetStockByProductIdAsync(Guid productId);
    }
}
