using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface ILgaRepository
    {
        Task<Lga?> GetByIdAsync(int id);
        Task<IEnumerable<Lga>> GetByStateIdAsync(int stateId);
    }
}
