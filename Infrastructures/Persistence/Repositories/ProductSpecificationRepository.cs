using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class ProductSpecificationRepository : IProductSpecificationRepository
    {
        private readonly BizStockContext _context;

        public ProductSpecificationRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<ProductSpecification?> GetByIdAsync(Guid id) =>
            await _context.ProductSpecifications
                          .Include(ps => ps.Specification) 
                          .FirstOrDefaultAsync(ps => ps.Id == id);

        public async Task<IEnumerable<ProductSpecification>> GetByProductIdAsync(Guid productId) =>
            await _context.ProductSpecifications
                          .Include(ps => ps.Specification)
                          .Where(ps => ps.ProductId == productId)
                          .ToListAsync();

        public async Task AddAsync(ProductSpecification productSpecification) =>
            await _context.ProductSpecifications.AddAsync(productSpecification);

        public async Task Update(ProductSpecification productSpecification)
        {
            _context.ProductSpecifications.Update(productSpecification);
            await Task.CompletedTask;
        }

        public async Task Remove(ProductSpecification productSpecification)
        {
            _context.ProductSpecifications.Remove(productSpecification);
            await Task.CompletedTask;
        }

        public async Task<ProductSpecification?> GetByProductAndSpecificationAsync(Guid productId, Guid specificationId)
        {
            return await _context.ProductSpecifications
                .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.SpecificationId == specificationId);
        }

    }
}
