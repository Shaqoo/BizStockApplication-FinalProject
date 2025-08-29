using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ISpecificationRepository
    {
        Task<Specification?> GetByIdAsync(Guid id);
        Task<Specification?> GetByNameAsync(string name);
        Task<IEnumerable<Specification>> GetAllAsync();
        Task AddAsync(Specification specification);
        Task Update(Specification specification);
        Task Remove(Specification specification);
    }
}
