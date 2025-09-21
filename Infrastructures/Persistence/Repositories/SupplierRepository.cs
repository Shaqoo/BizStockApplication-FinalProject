using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly BizStockContext _context;

        public SupplierRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Supplier supplier)
        {
            await _context.Suppliers.AddAsync(supplier);
        }

        public async Task<Supplier?> GetByIdAsync(Guid id)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(a => a.Id == id) ?? null;
        }

        public async Task<PaginatedList<Supplier>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Suppliers.AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(s => s.DateCreated)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Supplier>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Supplier>> FindAsync(Expression<Func<Supplier, bool>> predicate)
        {
            return await _context.Suppliers.Where(predicate).ToListAsync();
        }

        public async Task<Supplier?> GetByEmailAsync(string email)
        {
            return await _context.Suppliers
                .FirstOrDefaultAsync(s => s.Email == new Email(email));
        }

        public async Task UpdateSupplierAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await Task.CompletedTask;
        }

        public async Task DeleteSupplierAsync(Guid supplierId)
        {
            var supplier = await _context.Suppliers.FindAsync(supplierId)
                ?? throw new EntityNotFoundException("Supplier","Id");
            _context.Suppliers.Remove(supplier);
        }

        public async Task<PaginatedList<Supplier>> SearchSuppliersAsync(string keyword, PageRequest pageRequest)
        {
            var formattedKeyword = keyword.Trim().Replace(" ", " & ");

            var query = _context.Suppliers.Include(a => a.User)
                .Where(s =>
                    EF.Functions.ToTsVector("english", EF.Property<string>(s, "SearchVector"))
                    .Matches(EF.Functions.PlainToTsQuery("english", formattedKeyword)));

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(s => s.User.FullName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Supplier>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<Supplier?> GetByExpression(Expression<Func<Supplier, bool>> predicate)
        {
            return await _context.Suppliers.FirstOrDefaultAsync(predicate);
        }
    }

}
