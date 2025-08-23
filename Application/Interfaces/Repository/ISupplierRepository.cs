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
    public interface ISupplierRepository : IBaseRepository<Supplier>
    {
        Task<Supplier> GetByEmailAsync(string email);
        Task UpdateSupplierAsync(Supplier supplier);
        Task DeleteSupplierAsync(Guid supplierId);
        Task<PaginatedList<Supplier>> SearchSuppliersAsync(string keyword,PageRequest pageRequest);
    }
}
