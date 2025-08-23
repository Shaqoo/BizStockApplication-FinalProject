using Application.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IBrandRepository
    {
        Task<Brand?> GetByIdAsync(Guid id);
        Task<Brand?> GetByNameAsync(string name);
        Task<PaginatedList<Brand>> GetAllAsync(PageRequest pageRequest);
        Task<PaginatedList<Brand>> SearchAsync(string keyword,PageRequest pageRequest);
        Task AddAsync(Brand brand);
        void Update(Brand brand);
        void Delete(Brand brand);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> HasProduct(Guid brandId);
    }
}
