using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
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
    public class StockMovementRepository : IStockMovementRepository
    {
        private readonly BizStockContext _context;

        public StockMovementRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StockMovement entity)
        {
            await _context.StockMovements.AddAsync(entity);
        }

        public async Task<StockMovement?> GetByIdAsync(Guid id)
        {
            return await _context.StockMovements.Include(a => a.WarehouseItem).FirstAsync(a => a.Id == id)
                ?? throw new EntityNotFoundException("Stock movement","Id");
        }

        public async Task<PaginatedList<StockMovement>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.StockMovements.Include(a => a.WarehouseItem).AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(m => m.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<StockMovement>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<StockMovement>> FindAsync(Expression<Func<StockMovement, bool>> predicate)
        {
            return await _context.StockMovements.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<StockMovement>> GetByWarehouseItemIdAsync(Guid warehouseItemId)
        {
            return await _context.StockMovements
                .Where(m => m.WarehouseItemId == warehouseItemId)
                .OrderByDescending(m => m.Id)
                .ToListAsync();
        }

        public async Task<PaginatedList<StockMovementDto>> GetByDateRangeAsync(DateTime start, DateTime end, PageRequest pageRequest)
        {
            var query = _context.StockMovements.Include(a => a.WarehouseItem)
                .Where(m => m.DateCreated >= start && m.DateCreated <= end)
                .Select(a => a.ToDto());  

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(m => m.Date)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<int> GetTotalQuantityInAsync(Guid warehouseItemId)
        {
            return await _context.StockMovements
                .Where(m => m.WarehouseItemId == warehouseItemId && m.MovementType == StockMovementType.Inbound)
                .SumAsync(m => m.QuantityChanged);
        }

        public async Task<int> GetTotalQuantityOutAsync(Guid warehouseItemId)
        {
            return await _context.StockMovements
                .Where(m => m.WarehouseItemId == warehouseItemId && m.MovementType == StockMovementType.Outbound)
                .SumAsync(m => m.QuantityChanged);
        }

        public async Task<PaginatedList<StockMovementDto>> GetByWarehousePagedAsync(Guid warehouseItemId, PageRequest pageRequest)
        {
            var query = _context.StockMovements.Include(a => a.WarehouseItem).Where(a => a.WarehouseItem.WarehouseId == warehouseItemId).Select(a => a.ToDto()).AsQueryable();

            var items = await query.OrderBy(a => a.Date)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items,query.Count(),pageRequest.Page,pageRequest.PageSize);
        }

        public async Task<StockMovement> GetByExpression(Expression<Func<StockMovement, bool>> predicate)
        {
            return await _context.StockMovements.FirstOrDefaultAsync(predicate) ??
               throw new EntityNotFoundException("Stock Movement","Predicate");
        }

        public async Task<PaginatedList<StockMovementDto>> GetByProductId(Guid productId, PageRequest pageRequest)
        {
            var query = _context.StockMovements.Include(a => a.WarehouseItem).Where(a => a.WarehouseItem.ProductId == productId)
                .Select(a => a.ToDto()).AsQueryable();

            var items = await query.OrderBy(a => a.Date)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items, query.Count(), pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<StockMovementDto>> GetByMovementType(StockMovementType movementType, PageRequest pageRequest)
        {
            var query = _context.StockMovements.Include(a => a.WarehouseItem).Where(a => a.MovementType == movementType)
               .Select(a => a.ToDto()).AsQueryable();

            var items = await query.OrderBy(a => a.Date)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items, query.Count(), pageRequest.Page, pageRequest.PageSize);
        }
    }

}
