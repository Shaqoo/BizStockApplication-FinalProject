using Application.Dto;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly BizStockContext _context;

        public PurchaseOrderRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PurchaseOrder entity)
        {
            await _context.PurchaseOrders.AddAsync(entity);
        }

        public async Task<PurchaseOrder?> GetByIdAsync(Guid id)
        {
            return await _context.PurchaseOrders.Include(a => a.Items).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginatedList<PurchaseOrder>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.PurchaseOrders.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<PurchaseOrder>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<PurchaseOrder>> FindAsync(Expression<Func<PurchaseOrder, bool>> predicate)
        {
            return await _context.PurchaseOrders.Where(predicate).ToListAsync();
        }

        public async Task<PurchaseOrder?> GetByOrderNumberAsync(string orderNumber)
        {
            return await _context.PurchaseOrders.FirstOrDefaultAsync(p => p.OrderNumber == orderNumber);
        }

        public async Task<PaginatedList<PurchaseOrderListDto>> GetBySupplierIdAsync(Guid supplierId, PageRequest pageRequest)
        {
            var query = _context.PurchaseOrders
                .Where(p => p.SupplierId == supplierId)
                .Include(a => a.Supplier)
                .ThenInclude(s => s.User)
                .AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(po => new PurchaseOrderListDto
                {
                    Id = po.Id,
                    PONumber = po.OrderNumber,
                    SupplierName = po.Supplier.User.FullName,
                    Status = po.Status,
                    CreatedAt = po.DateCreated,
                    TotalAmount = po.Total
                })
                .ToListAsync();
            return new PaginatedList<PurchaseOrderListDto>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PurchaseOrder?> GetWithItemsAsync(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == purchaseOrderId);
        }

        public async Task<decimal> GetTotalAmountForSupplierAsync(Guid supplierId)
        {
            return await _context.PurchaseOrders
                .Where(p => p.SupplierId == supplierId)
                .SumAsync(p => p.Total);
        }

        public async Task<decimal> GetTotalOutstandingAmountAsync(Guid purchaseOrderId)
        {
            var po = await _context.PurchaseOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.Id == purchaseOrderId);

            if (po == null)
                throw new KeyNotFoundException("Purchase order not found.");

            var receivedAmount = po.Items.Sum(i => i.QuantityReceived * i.UnitPrice);
            return po.Total - receivedAmount;
        }

        public async Task UpdateStatusAsync(PurchaseOrder purchaseOrder)
        {
            _context.PurchaseOrders.Update(purchaseOrder);
            await Task.CompletedTask;
        }

        public async Task<int> CountByStatusAsync(PurchaseOrderStatus status)
        {
            return await _context.PurchaseOrders.CountAsync(p => p.Status == status);
        }

        public async Task<PurchaseOrder?> GetByExpression(Expression<Func<PurchaseOrder, bool>> predicate)
        {
            return await _context.PurchaseOrders.FirstOrDefaultAsync(predicate);
        }

        public async Task<string> GetLastOrderNumber()
        {
            var lastOrder = await _context.PurchaseOrders
                .OrderByDescending(o => o.DateCreated)
                .FirstOrDefaultAsync();

            return lastOrder?.OrderNumber ?? "PO-00000";
        }

        public async Task<string> GenerateNextOrderNumber()
        {
            var lastNumber = await GetLastOrderNumber();

            var numberPart = lastNumber.Split('-').Last();
            if (int.TryParse(numberPart, out int numericValue))
            {
                numericValue++;
                return $"PO-{numericValue:D6}";
            }

            return "PO-00001";
        }

        public async Task<PurchaseOrderDetailDto?> GetPurchaseOrderDetailsById(Guid purchaseOrderId)
        {
            return await _context.PurchaseOrders
                .Where(po => po.Id == purchaseOrderId)
                .Include(po => po.Supplier)
                    .ThenInclude(s => s.User)
                .Include(po => po.Items)
                    .ThenInclude(i => i.Product)
                .Select(po => new PurchaseOrderDetailDto
                {
                    Id = po.Id,
                    PONumber = po.OrderNumber,
                    SupplierName = po.Supplier.User.FullName,
                    CreatedAt = po.DateCreated,
                    Status = po.Status,
                    TotalAmount = po.Total,
                    Items = po.Items.Select(i => new PurchaseOrderItemDto
                    {
                        Id = i.Id,
                        ProductName = i.ProductName,
                        OrderedQuantity = i.QuantityOrdered,
                        ReceivedQuantity = i.QuantityReceived,
                        UnitPrice = i.UnitPrice,
                        ProductId = i.ProductId,
                        ProductImgUrl = i.Product.ImageUrl
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<PaginatedList<PurchaseOrderListDto>> GetAllWithDtoAsync(PageRequest pageRequest)
        {
            var query = _context.PurchaseOrders
                .Include(po => po.Supplier)
                .ThenInclude(s => s.User)
                .AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(o => o.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(po => new PurchaseOrderListDto
                {
                    Id = po.Id,
                    PONumber = po.OrderNumber,
                    SupplierName = po.Supplier.User.FullName,
                    Status = po.Status,
                    CreatedAt = po.DateCreated,
                    TotalAmount = po.Total
                })
                .ToListAsync();

            return new PaginatedList<PurchaseOrderListDto>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<PurchaseOrderListDto>> FilterPurchaseOrderWithStatusPagedAsync(
        PurchaseOrderStatus purchaseOrderStatus,
        PageRequest pageRequest)
        {
            var query = _context.PurchaseOrders
                .Include(po => po.Supplier)
                .ThenInclude(s => s.User)
                .Where(po => po.Status == purchaseOrderStatus)
                .AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(po => po.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(po => new PurchaseOrderListDto
                {
                    Id = po.Id,
                    PONumber = po.OrderNumber,
                    SupplierName = po.Supplier.User.FullName,
                    Status = po.Status,
                    CreatedAt = po.DateCreated,
                    TotalAmount = po.Total
                })
                .ToListAsync();

            return new PaginatedList<PurchaseOrderListDto>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PurchaseOrderStatsDto> GetPurchaseOrderStatsAsync()
        {
            var query = _context.PurchaseOrders.AsQueryable();

            var totalPurchaseOrders = await query.CountAsync();

            var draftCount = await query.CountAsync(po => po.Status == PurchaseOrderStatus.Draft);
            var confirmedCount = await query.CountAsync(po => po.Status == PurchaseOrderStatus.Confirmed);
            var receivedCount = await query.CountAsync(po => po.Status == PurchaseOrderStatus.Received);
            var cancelledCount = await query.CountAsync(po => po.Status == PurchaseOrderStatus.Cancelled);
            var partiallyReceivedCount = await query.CountAsync(po => po.Status == PurchaseOrderStatus.PartiallyReceived);
            var rejectedCount = await query.CountAsync(po => po.Status == PurchaseOrderStatus.Received);


            var totalSpend = await query
                .Where(po => po.Status == PurchaseOrderStatus.Received || po.Status == PurchaseOrderStatus.PartiallyReceived)
                .SumAsync(po => (decimal?)(po.SubTotal - po.Discount + po.Tax)) ?? 0m;

            var outstandingAmount = await query
                .Where(po => po.Status == PurchaseOrderStatus.Confirmed || po.Status == PurchaseOrderStatus.PartiallyReceived)
                .SumAsync(po => (decimal?)(po.SubTotal - po.Discount + po.Tax)) ?? 0m;


            return new PurchaseOrderStatsDto
            {
                TotalPurchaseOrders = totalPurchaseOrders,
                DraftCount = draftCount,
                ConfirmedCount = confirmedCount,
                ReceivedCount = receivedCount,
                CancelledCount = cancelledCount,
                PartiallyReceivedCount = partiallyReceivedCount,
                RejectedCount = rejectedCount,
                TotalSpend = totalSpend,
                OutstandingAmount = outstandingAmount
            };
        }

        public async Task<PaginatedList<PurchaseOrderListDto>> GetPurchaseOrdersByDateRangeAsync(DateTime startDate, DateTime endDate, PageRequest pageRequest)
        {
            var startDateOffset = new DateTimeOffset(startDate, TimeSpan.Zero);
            var endDateOffset = new DateTimeOffset(endDate, TimeSpan.Zero);

            var query = _context.PurchaseOrders
                .Include(po => po.Supplier)
                .ThenInclude(s => s.User)
                 .Where(po => po.DateCreated >= startDateOffset && po.DateCreated <= endDateOffset)
                .AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(po => po.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(po => new PurchaseOrderListDto
                {
                    Id = po.Id,
                    PONumber = po.OrderNumber,
                    SupplierName = po.Supplier.User.FullName,
                    Status = po.Status,
                    CreatedAt = po.DateCreated,
                    TotalAmount = po.Total
                })
                .ToListAsync();

            return new PaginatedList<PurchaseOrderListDto>(items, total, pageRequest.Page, pageRequest.PageSize);
        }
    }

}
