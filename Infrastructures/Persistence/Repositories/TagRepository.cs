using Application.Dto;
using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Persistence.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BizStockContext _context;

        public TagRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task<Tag?> GetByIdAsync(Guid id)
        {
            return await _context.Tags.FindAsync(id);
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task<PaginatedList<TagDto>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.Tags.AsQueryable();

            var itemCount = await query.CountAsync();
            var items = await query
                .OrderBy(t => t.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(t => new TagDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();

            return new PaginatedList<TagDto>(items, itemCount, pageRequest.Page, pageRequest.PageSize);

        }

        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public async Task UpdateAsync(Tag tag)
        {
            _context.Tags.Update(tag);
        }

        public async Task DeleteAsync(Tag tag)
        {
            _context.Tags.Remove(tag);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Tags
                .AnyAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task<PaginatedList<TagDto>> GetByProductIdAsync(Guid productId, PageRequest pageRequest)
        {
            var query = _context.Tags
                .Where(t => t.ProductTags.Any(p => p.ProductId == productId))
                .AsQueryable();

            var count = await query.CountAsync();

            var items = await query
                .OrderBy(t => t.Name)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .Select(t => new TagDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();

            return new PaginatedList<TagDto>(items, count, pageRequest.Page, pageRequest.PageSize);
        }
    }

}
