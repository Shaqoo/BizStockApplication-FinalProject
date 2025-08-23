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
    public interface IWarehouseItemRepository : IBaseRepository<WarehouseItem>
    {
        Task<PaginatedList<WarehouseItem>> GetItemsByWarehouseIdAsync(Guid warehouseId,PageRequest pageRequest);
        Task UpdateWarehouseItemAsync(WarehouseItem warehouseItem);
        Task DeleteWarehouseItemAsync(Guid itemId);
        Task<bool> IsItemInWarehouseAsync(Guid itemId, Guid warehouseId);
        Task<PaginatedList<WarehouseItem>> SearchItemsAsync(string keyword, Guid warehouseId,PageRequest pageRequest);
    }
}
