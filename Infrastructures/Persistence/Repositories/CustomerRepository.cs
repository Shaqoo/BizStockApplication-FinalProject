using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BizStockContext _context;

        public CustomerRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.Include(a => a.DeliveryAddresses)
                .ThenInclude(a => a.State)
                .ThenInclude(a => a.Lgas)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PaginatedList<Customer>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Customers.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.FullName)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Customer>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<Customer>> FindAsync(Expression<Func<Customer, bool>> predicate)
        {
            return await _context.Customers.Where(predicate).ToListAsync();
        }

        public async Task UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            await Task.CompletedTask; 
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers.Include(a => a.CustomerType)
                .FirstOrDefaultAsync(c => c.Email == new Email(email));
        }

        public async Task<Customer?> GetByExpression(Expression<Func<Customer, bool>> predicate)
        {
            return await _context.Customers.FirstOrDefaultAsync(predicate);
        }
    }

}
