using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface ICustomerTypeRepository : IBaseRepository<CustomerType>
    {
        Task<CustomerType> GetByNameAsync(CustomerTypeName name);
        Task<bool> IsNameUniqueAsync(string name);
        Task UpdateCustomerTypeAsync(CustomerType customerType);
        Task DeleteCustomerTypeAsync(Guid customerTypeId);
    }
}
