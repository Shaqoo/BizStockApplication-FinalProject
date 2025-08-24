using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ISupplierRepository : IBaseRepository<Supplier>
    {
        Task<Supplier?> GetByEmailAsync(string email);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(Guid supplierId);
        Task<PaginatedList<Supplier>> SearchSuppliersAsync(string keyword,PageRequest pageRequest);
    }
}
