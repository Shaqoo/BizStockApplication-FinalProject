using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Persistence.Repositories
{
 
    public class BrandRepository : IBrandRepository
    {
        private readonly BizStockContext _context;

        public BrandRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<Brand?> GetByIdAsync(Guid id)
        {
            return await _context.Brands
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Brand?> GetByNameAsync(string name)
        {
            return await _context.Brands
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower());
        }

        public async Task<PaginatedList<Brand>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Brands.AsNoTracking().AsQueryable();

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(b => b.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize).ToListAsync();

            return new PaginatedList<Brand>(items,total ,pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<PaginatedList<Brand>> SearchAsync(string keyword, PageRequest pageRequest)
        {
            keyword = keyword.Trim().ToLower();

            IQueryable<Brand> query = _context.Brands.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(b =>
                    EF.Functions.ILike(b.Name, $"%{keyword}%") ||
                    (b.Description != null && EF.Functions.ILike(b.Description, $"%{keyword}%"))
                );
            }

            var total = await query.CountAsync();

            var items = await query.OrderBy(b => b.Name)
                    .Skip(pageRequest.PageSize * (pageRequest.Page - 1))
                    .Take(pageRequest.PageSize)
                    .ToListAsync();

            return new PaginatedList<Brand>(items, total, pageRequest.Page, pageRequest.PageSize);
        }


        public async Task AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
        }

        public void Update(Brand brand)
        {
            _context.Brands.Update(brand);
        }

        public void Delete(Brand brand)
        {
            _context.Brands.Remove(brand);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Brands.AnyAsync(b => b.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> HasProduct(Guid brandId)
        {
            return await _context.Products
                .AsNoTracking()
                .AnyAsync(p => p.BrandId == brandId);
        }
    }
}


