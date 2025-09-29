using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IStateRepository
    {
        Task<State?> GetByIdAsync(int id);
        Task<IEnumerable<State>> GetAllAsync();
    }
}
