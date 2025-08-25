using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class WarehouseItemRepository : IWarehouseItemRepository
    {
        private readonly BizStockContext _context;

        public WarehouseItemRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WarehouseItem item)
        {
            await _context.WarehouseItems.AddAsync(item);
        }

        public async Task<WarehouseItem?> GetByIdAsync(Guid id)
        {
            return await _context.WarehouseItems.FindAsync(id);
        }

        public async Task<PaginatedList<WarehouseItem>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.WarehouseItems.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<WarehouseItem>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<WarehouseItem>> FindAsync(Expression<Func<WarehouseItem, bool>> predicate)
        {
            return await _context.WarehouseItems
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<PaginatedList<WarehouseItem>> GetItemsByWarehouseIdAsync(Guid warehouseId, PageRequest pageRequest)
        {
            var query = _context.WarehouseItems.Include(a => a.Product)
                .Include(a => a.Warehouse)
                .Where(w => w.WarehouseId == warehouseId);

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(w => w.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<WarehouseItem>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task UpdateWarehouseItemAsync(WarehouseItem warehouseItem)
        {
            _context.WarehouseItems.Update(warehouseItem);
            await Task.CompletedTask;
        }

        public async Task DeleteWarehouseItemAsync(Guid itemId)
        {
            var item = await _context.WarehouseItems.FindAsync(itemId);
            if (item != null)
            {
                _context.WarehouseItems.Remove(item);
            }
            await Task.CompletedTask;
        }

        public async Task<bool> IsItemInWarehouseAsync(Guid itemId, Guid warehouseId)
        {
            return await _context.WarehouseItems
                .AnyAsync(i => i.Id == itemId && i.WarehouseId == warehouseId);
        }

        public async Task<PaginatedList<WarehouseItem>> SearchItemsAsync(string keyword, Guid warehouseId, PageRequest pageRequest)
        {
            var formattedKeyword = keyword.Trim().Replace(" ", " & ");

            var query = _context.WarehouseItems
                .Where(i => i.WarehouseId == warehouseId &&
                    EF.Functions.ToTsVector("english", EF.Property<string>(i, "SearchVector"))
                    .Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword)));

            var total = await query.CountAsync();

            if (total == 0)
            {
                query = _context.WarehouseItems
                    .Where(i => i.WarehouseId == warehouseId &&
                        EF.Functions.ILike(i.Product.Name, $"%{keyword}%"));

                total = await query.CountAsync();
            }

            var items = await query
                .OrderBy(i => i.Product.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<WarehouseItem>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<WarehouseItem?> GetByExpression(Expression<Func<WarehouseItem, bool>> predicate)
        {
            return await _context.WarehouseItems.Include(a => a.Warehouse).Include(a => a.Product)
                .FirstOrDefaultAsync(predicate) ??
               null;
        }
    }

}
