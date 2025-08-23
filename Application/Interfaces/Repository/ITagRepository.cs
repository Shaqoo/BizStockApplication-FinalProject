using Application.Dto;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ITagRepository
    {
        Task<Tag?> GetByIdAsync(Guid id);
        Task<Tag?> GetByNameAsync(string name);
        Task<PaginatedList<TagDto>> GetAllAsync(PageRequest pageRequest);
        Task<PaginatedList<TagDto>> GetByProductIdAsync(Guid productId, PageRequest pageRequest);
        Task AddAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(Tag tag);
        Task<bool> ExistsByNameAsync(string name);
    }

}
