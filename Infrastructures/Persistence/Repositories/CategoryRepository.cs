using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BizStockContext _context;

        public CategoryRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories
                .Include(x => x.SubCategories)
                .Include(x => x.ParentCategory)
                .Where(c => c.Id == id && !c.IsDeleted)
                .Select(c => new CategoryDto(
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Depth,
                    c.ParentCategoryId,
                    c.Products.Count()
                ))
                .FirstOrDefaultAsync();
        }


        public async Task<PaginatedList<Category>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Categories.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Category>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<List<Category>> GetAllCategoriesWithHierarchyAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Include(x => x.SubCategories)
                .Include(x => x.ParentCategory)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }


        public async Task<IEnumerable<Category>> FindAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _context.Categories.Where(predicate).ToListAsync();
        }

        public async Task<Category> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower())
                ?? throw new KeyNotFoundException("Category not found by name.");
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return !await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<int> GetProductCountAsync(Guid categoryId)
        {
            return await _context.Products.CountAsync(p => p.CategoryId == categoryId);
        }

        public async Task<bool> HasProductsAsync(Guid categoryId)
        {
            return await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
        }

        public async Task<Category?> GetByExpression(Expression<Func<Category, bool>> predicate)
        {
             return await _context.Categories
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<PaginatedList<CategoryDto>> GetFilteredCategoriesAsync(GetCategoriesFilter filter, CancellationToken cancellationToken = default)
        {
            var query = _context.Categories.AsNoTracking();

            if (filter.Depth.HasValue)
                query = query.Where(c => c.Depth == filter.Depth);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                query = query.Where(c => c.Name.ToLower().Contains(filter.SearchTerm.ToLower()));

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(c => c.Name)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(c => new CategoryDto
                (
                 c.Id,
                 c.Name,
                 c.Description,
                 c.Depth,
                 c.ParentCategoryId,
                 c.Products.Count()
                ))
                .ToListAsync(cancellationToken);

            return new PaginatedList<CategoryDto>
            {
                Items = items,
                TotalCount = total,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public Task<Category?> GetByIdAsync(Guid id)
        {
            return _context.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }
    }

}
