using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IFidoCredentialRepository : IBaseRepository<FidoCredential>
    {
        Task<List<FidoCredential>> GetByUserIdAsync(Guid userId);
        Task<FidoCredential?> GetByCredentialIdAsync(string credentialId);
        Task UpdateFidoCredentialAsync(FidoCredential fidoCredential);
        Task DeleteFidoCredentialAsync(Guid userId);
    }
}
