using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.Enums;
using Infrastructures.Persistence.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly BizStockContext _context;

        public CustomerTypeRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CustomerType customerType)
        {
            await _context.CustomerTypes.AddAsync(customerType);
        }

        public async Task<CustomerType?> GetByIdAsync(Guid id)
        {
            return await _context.CustomerTypes.FindAsync(id)
                ?? throw new KeyNotFoundException("Customer type not found.");
        }

        public async Task<PaginatedList<CustomerType>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.CustomerTypes.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(ct => ct.TypeName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<CustomerType>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<CustomerType>> FindAsync(Expression<Func<CustomerType, bool>> predicate)
        {
            return await _context.CustomerTypes.Where(predicate).ToListAsync();
        }

        public async Task<CustomerType> GetByNameAsync(CustomerTypeName name)
        {
            return await _context.CustomerTypes
                .FirstOrDefaultAsync(c => c.TypeName == name)
                ?? throw new KeyNotFoundException("Customer type not found.");
        }


        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.CustomerTypes.AnyAsync(ct => ct.TypeName.ToString().ToLower() == name.ToLower());
        }

        public async Task UpdateCustomerTypeAsync(CustomerType customerType)
        {
            _context.CustomerTypes.Update(customerType);
            await Task.CompletedTask; 
        }

        public async Task DeleteCustomerTypeAsync(Guid customerTypeId)
        {
            var entity = await _context.CustomerTypes.FindAsync(customerTypeId);

            if (entity == null)
                throw new KeyNotFoundException("Customer type not found.");

            _context.CustomerTypes.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<CustomerType> GetByExpression(Expression<Func<CustomerType, bool>> predicate)
        {
            return await _context.CustomerTypes.FirstOrDefaultAsync(predicate) ??
                throw new ArgumentNullException("Customer Type Not Found");
        }
    }

}
