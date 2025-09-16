using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class SpecificationRepository : ISpecificationRepository
    {
        private readonly BizStockContext _context;

        public SpecificationRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<Specification?> GetByIdAsync(Guid id) =>
            await _context.Specifications.Include(a => a.ProductSpecifications).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<Specification>> GetAllAsync() =>
            await _context.Specifications.ToListAsync();

        public async Task AddAsync(Specification specification) =>
            await _context.Specifications.AddAsync(specification);

        public async Task Update(Specification specification)
        {
            _context.Specifications.Update(specification);
            await Task.CompletedTask;
        }

        public async Task Remove(Specification specification)
        {
            _context.Specifications.Remove(specification);
            await Task.CompletedTask;
        }

        public async Task<Specification?> GetByNameAsync(string name)
         => await _context.Specifications.FirstOrDefaultAsync(a => a.Name == name);
    }
}
