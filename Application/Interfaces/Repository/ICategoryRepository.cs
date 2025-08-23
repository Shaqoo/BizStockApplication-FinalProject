using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<Category> GetByNameAsync(string name);
        Task<bool> IsNameUniqueAsync(string name);
        Task<int> GetProductCountAsync(Guid categoryId);
        Task<bool> HasProductsAsync(Guid categoryId);
        Task<PaginatedList<CategoryDto>> GetFilteredCategoriesAsync(GetCategoriesFilter filter, CancellationToken cancellationToken = default);
        Task<List<Category>> GetAllCategoriesWithHierarchyAsync(CancellationToken cancellationToken = default);
    }
}
