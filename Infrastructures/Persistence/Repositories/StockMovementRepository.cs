using Application.Dto;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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

        public async Task<StockMovement?> GetByExpression(Expression<Func<StockMovement, bool>> predicate)
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

        public async Task<StockMovementStatsDto> GetStockMovementStatsAsync()
        {
            var totalInbound = await _context.StockMovements
                .CountAsync(sm => sm.MovementType == StockMovementType.Inbound);

            var totalOutbound = await _context.StockMovements
                .CountAsync(sm => sm.MovementType == StockMovementType.Outbound);

            var totalAdjustmentIn = await _context.StockMovements
                .CountAsync(sm => sm.MovementType == StockMovementType.AdjustmentIn);

            var totalAdjustmentOut = await _context.StockMovements
                .CountAsync(sm => sm.MovementType == StockMovementType.AdjustmentOut);

            var totalTransferIn = await _context.StockMovements
                .CountAsync(sm => sm.MovementType == StockMovementType.TransferIn);

            var totalTransferOut = await _context.StockMovements
                .CountAsync(sm => sm.MovementType == StockMovementType.TransferOut);

            var totalMovements = totalInbound + totalOutbound + totalAdjustmentIn +
                                 totalAdjustmentOut + totalTransferIn + totalTransferOut;

            return new StockMovementStatsDto(totalInbound, totalOutbound, totalAdjustmentIn, totalAdjustmentOut, totalTransferIn,
                totalTransferOut, totalMovements);
        }

        public async Task<List<StockMovementTrendDto>> GetStockMovementTrendAsync(string range)
        {
            var now = DateTime.UtcNow.Date;
            var query = _context.StockMovements.AsNoTracking();

            DateTime fromDate = range.ToLower() switch
            {
                "daily" => now.AddDays(-9),   
                "weekly" => now.AddDays(-7 * 5), 
                "monthly" => now.AddMonths(-5),  
                _ => throw new ArgumentException("Invalid range. Use 'daily', 'weekly' or 'monthly'.")
            };

            query = query.Where(sm => sm.DateCreated >= fromDate);

            List<StockMovementTrendDto> grouped;

            switch (range.ToLower())
            {
                case "daily":
                    grouped = await query
                        .GroupBy(sm => sm.DateCreated.Date)
                        .Select(g => new StockMovementTrendDto
                        {
                            Period = g.Key.ToString("yyyy-MM-dd"),
                            Inbound = g.Count(x => x.MovementType == StockMovementType.Inbound),
                            Outbound = g.Count(x => x.MovementType == StockMovementType.Outbound),
                            AdjustmentIn = g.Count(x => x.MovementType == StockMovementType.AdjustmentIn),
                            AdjustmentOut = g.Count(x => x.MovementType == StockMovementType.AdjustmentOut),
                            TransferIn = g.Count(x => x.MovementType == StockMovementType.TransferIn),
                            TransferOut = g.Count(x => x.MovementType == StockMovementType.TransferOut)
                        })
                        .ToListAsync();
                    break;

                case "weekly":
                    var weeklyData = await query.ToListAsync();
                    grouped = weeklyData
                        .GroupBy(sm => new { sm.DateCreated.Year, Week = ISOWeek.GetWeekOfYear(sm.DateCreated.DateTime) })
                        .Select(g => new StockMovementTrendDto
                        {
                            Period = $"{g.Key.Year}-W{g.Key.Week:D2}",
                            Inbound = g.Count(x => x.MovementType == StockMovementType.Inbound),
                            Outbound = g.Count(x => x.MovementType == StockMovementType.Outbound),
                            AdjustmentIn = g.Count(x => x.MovementType == StockMovementType.AdjustmentIn),
                            AdjustmentOut = g.Count(x => x.MovementType == StockMovementType.AdjustmentOut),
                            TransferIn = g.Count(x => x.MovementType == StockMovementType.TransferIn),
                            TransferOut = g.Count(x => x.MovementType == StockMovementType.TransferOut)
                        })
                        .ToList();
                    break;

                case "monthly":
                    grouped = await query
                        .GroupBy(sm => new { sm.DateCreated.Year, sm.DateCreated.Month })
                        .Select(g => new StockMovementTrendDto
                        {
                            Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                            Inbound = g.Count(x => x.MovementType == StockMovementType.Inbound),
                            Outbound = g.Count(x => x.MovementType == StockMovementType.Outbound),
                            AdjustmentIn = g.Count(x => x.MovementType == StockMovementType.AdjustmentIn),
                            AdjustmentOut = g.Count(x => x.MovementType == StockMovementType.AdjustmentOut),
                            TransferIn = g.Count(x => x.MovementType == StockMovementType.TransferIn),
                            TransferOut = g.Count(x => x.MovementType == StockMovementType.TransferOut)
                        })
                        .ToListAsync();
                    break;

                default:
                    throw new ArgumentException("Invalid range. Use 'daily', 'weekly', or 'monthly'.");
            }

          
            var dict = grouped.ToDictionary(x => x.Period);

            List<StockMovementTrendDto> final = range.ToLower() switch
            {
                "daily" => Enumerable.Range(0, 10)
                    .Select(i => now.AddDays(-9 + i))
                    .Select(date => dict.TryGetValue(date.ToString("yyyy-MM-dd"), out var dto)
                        ? dto
                        : new StockMovementTrendDto { Period = date.ToString("yyyy-MM-dd") })
                    .ToList(),

                "weekly" => Enumerable.Range(0, 6)
                    .Select(i =>
                    {
                        var start = now.AddDays(-7 * (5 - i));
                        var week = ISOWeek.GetWeekOfYear(start);
                        return $"{start.Year}-W{week:D2}";
                    })
                    .Select(period => dict.TryGetValue(period, out var dto)
                        ? dto
                        : new StockMovementTrendDto { Period = period })
                    .ToList(),

                "monthly" => Enumerable.Range(0, 6)
                    .Select(i => now.AddMonths(-5 + i))
                    .Select(d => $"{d.Year}-{d.Month:D2}")
                    .Select(period => dict.TryGetValue(period, out var dto)
                        ? dto
                        : new StockMovementTrendDto { Period = period })
                    .ToList(),

                _ => grouped
            };

            return final.OrderBy(x => x.Period).ToList();
        }

    }

}
