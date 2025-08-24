using Application.Dto;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly BizStockContext _context;

        public WarehouseRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Warehouse warehouse)
        {
            await _context.Warehouses.AddAsync(warehouse);
        }

        public async Task<Warehouse?> GetByIdAsync(Guid id)
        {
            return await _context.Warehouses.FindAsync(id)
                ?? throw new EntityNotFoundException("Warehouse", "Id");
        }

        public async Task<PaginatedList<WarehouseDto>> GetAllAsyncWithDto(PageRequest pageRequest)
        {
            var query = _context.Warehouses.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(w => w.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(w => new WarehouseDto(
                    w.Id,
                    w.Name,
                    w.Location,
                    w.IsActive,
                    w.Items.Count
                ))
                .ToListAsync();

            return new PaginatedList<WarehouseDto>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Warehouse>> FindAsync(Expression<Func<Warehouse, bool>> predicate)
        {
            return await _context.Warehouses.AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<Warehouse> GetByLocationAsync(string location)
        {
            return await _context.Warehouses
                .FirstOrDefaultAsync(w => w.Location == location)
                ?? throw new EntityNotFoundException("Warehouse","Location");
        }

        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            _context.Warehouses.Update(warehouse);
            await Task.CompletedTask;
        }

        public async Task DeleteWarehouseAsync(Guid warehouseId)
        {
            var warehouse = await _context.Warehouses.FindAsync(warehouseId);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
            }
            await Task.CompletedTask;
        }

        public async Task<bool> IsNameUnique(string name)
        {
            return await _context.Warehouses.AsNoTracking().AnyAsync(w => w.Name == name);
        }

        public async Task<PaginatedList<WarehouseDto>> SearchWarehousesAsync(string keyword,PageRequest pageRequest)
        {
            var formatted = keyword.Trim().Replace(" ", " & ");

            
            var query = _context.Warehouses.AsNoTracking()
                    .Where(w => EF.Functions.ILike(w.Name, $"%{keyword}%") ||
                                EF.Functions.ILike(w.Location, $"%{keyword}%"));

             int  total = await query.CountAsync();
            

            var items = await query
                .OrderBy(w => w.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(w => new WarehouseDto(
                    w.Id,
                    w.Name,
                    w.Location,
                    w.IsActive,
                    w.Items.Count
                ))
                .ToListAsync();

            return new PaginatedList<WarehouseDto>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<Warehouse?> GetByExpression(Expression<Func<Warehouse, bool>> predicate)
        {
            return await _context.Warehouses.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> HasItemAsync(Guid warehouseId)
        {
            return await _context.WarehouseItems.AsNoTracking().AnyAsync(i => i.Id == warehouseId);
        }

        public async Task<int> GetCount(Guid warehouseId)
        {
            return await _context.WarehouseItems.AsNoTracking().CountAsync(i => i.Id == warehouseId);
        }


        public Task<PaginatedList<Warehouse>> GetAllAsync(PageRequest pageRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(Guid Id)
        {
            return await _context.Warehouses.AsNoTracking().AnyAsync(w => w.Id == Id);
        }
    }

}
