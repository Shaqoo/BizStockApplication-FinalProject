using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
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
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private readonly BizStockContext _context;

        public InvoiceItemRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InvoiceItem item)
        {
            await _context.InvoiceItems.AddAsync(item);
        }

        public async Task AddRangeAsync(IEnumerable<InvoiceItem> items)
        {
            await _context.InvoiceItems.AddRangeAsync(items);
        }

        public async Task<InvoiceItem?> GetByIdAsync(Guid id)
        {
            return await _context.InvoiceItems.FindAsync(id)
                ?? throw new KeyNotFoundException("Invoice item not found.");
        }

        public async Task<PaginatedList<InvoiceItem>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.InvoiceItems.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.Id)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<InvoiceItem>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<InvoiceItem>> FindAsync(Expression<Func<InvoiceItem, bool>> predicate)
        {
            return await _context.InvoiceItems.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(Guid invoiceId)
        {
            return await _context.InvoiceItems
                .Where(i => i.InvoiceId == invoiceId)
                .ToListAsync();
        }

        public async Task<InvoiceItem> GetByExpression(Expression<Func<InvoiceItem, bool>> predicate)
        {
            return await _context.InvoiceItems.FirstOrDefaultAsync(predicate) ??
                throw new ArgumentNullException("Invoice Item Was Not Found");
        }
    }

}
