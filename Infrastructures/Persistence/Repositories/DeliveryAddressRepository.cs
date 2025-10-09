using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructures.Persistence.Repositories
{
    public class DeliveryAddressRepository : IDeliveryAddressRepository
    {
        private readonly BizStockContext _context;
        public DeliveryAddressRepository(BizStockContext context) => _context = context;

        public async Task<DeliveryAddress?> GetByIdAsync(Guid id) =>
            await _context.DeliveryAddresses
                .Include(a => a.State)
                .Include(a => a.Lga)
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<DeliveryAddress>> GetByCustomerIdAsync(Guid customerId) =>
            await _context.DeliveryAddresses
                .Include(a => a.State)
                .Include(a => a.Lga)
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();

        public async Task AddAsync(DeliveryAddress address)
        {
            await _context.DeliveryAddresses.AddAsync(address);
        }

        public async Task UpdateAsync(DeliveryAddress address)
        {
            _context.DeliveryAddresses.Update(address);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.DeliveryAddresses.FindAsync(id);
            if (entity != null)
            {
                _context.DeliveryAddresses.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<DeliveryAddress?> GetDefaultAsync(Guid customerId) =>
            await _context.DeliveryAddresses
                .Include(a => a.State)
                .Include(a => a.Lga)
                .FirstOrDefaultAsync(a => a.CustomerId == customerId && a.IsDefault);

    }
}
