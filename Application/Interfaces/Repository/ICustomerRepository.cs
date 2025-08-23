using Application.Interfaces.Repository.BaseRepository;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task UpdateCustomer(Customer customer);
        Task<Customer> GetByEmailAsync(string email);   
    }
}
