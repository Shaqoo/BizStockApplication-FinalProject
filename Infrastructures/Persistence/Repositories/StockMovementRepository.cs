using Application.Dto;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
            var startUtc = new DateTimeOffset(start, TimeSpan.Zero);
            var endUtc = new DateTimeOffset(end, TimeSpan.Zero);

            var baseQuery = _context.StockMovements
                .Include(a => a.WarehouseItem)
                .Where(m => m.DateCreated >= startUtc && m.DateCreated <= endUtc);


            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderBy(a => a.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(a => new StockMovementDto(
                    a.Id,
                    a.WarehouseItem.ProductId,
                    a.MovementType,
                    a.QuantityChanged,
                    a.WarehouseItem.WarehouseId,
                    a.DateCreated,
                    a.PerformedByUserId,
                    a.Reason
                ))
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
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
            var baseQuery = _context.StockMovements
                .Include(a => a.WarehouseItem)
                .Where(a => a.WarehouseItem.WarehouseId == warehouseItemId);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderBy(a => a.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(a => new StockMovementDto(
                    a.Id,
                    a.WarehouseItem.ProductId,
                    a.MovementType,
                    a.QuantityChanged,
                    a.WarehouseItem.WarehouseId,
                    a.DateCreated,
                    a.PerformedByUserId,
                    a.Reason
                ))
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<StockMovement> GetByExpression(Expression<Func<StockMovement, bool>> predicate)
        {
            return await _context.StockMovements.FirstOrDefaultAsync(predicate) ??
               throw new EntityNotFoundException("Stock Movement","Predicate");
        }

        public async Task<PaginatedList<StockMovementDto>> GetByProductId(Guid productId, PageRequest pageRequest)
        {
            var baseQuery = _context.StockMovements
                .Include(a => a.WarehouseItem)
                .Where(a => a.WarehouseItem.ProductId == productId);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderBy(a => a.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(a => new StockMovementDto(
                    a.Id,
                    a.WarehouseItem.ProductId,
                    a.MovementType,
                    a.QuantityChanged,
                    a.WarehouseItem.WarehouseId,
                    a.DateCreated,
                    a.PerformedByUserId,
                    a.Reason
                ))
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<StockMovementDto>> GetByMovementType(StockMovementType movementType, PageRequest pageRequest)
        {
            var baseQuery = _context.StockMovements
                .Include(a => a.WarehouseItem)
                .Where(a => a.MovementType == movementType);

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderBy(a => a.DateCreated) 
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(a => new StockMovementDto(
                    a.Id,
                    a.WarehouseItem.ProductId,
                    a.MovementType,
                    a.QuantityChanged,
                    a.WarehouseItem.WarehouseId,
                    a.DateCreated,
                    a.PerformedByUserId,
                    a.Reason
                ))
                .ToListAsync();

            return new PaginatedList<StockMovementDto>(items, totalCount, pageRequest.Page, pageRequest.PageSize);
        }

    }

}
