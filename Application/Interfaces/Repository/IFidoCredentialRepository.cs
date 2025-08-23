using Application.Interfaces.Repository.BaseRepository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IFidoCredentialRepository : IBaseRepository<FidoCredential>
    {
        Task<List<FidoCredential>> GetByUserIdAsync(Guid userId);
        Task<FidoCredential> GetByCredentialIdAsync(string credentialId);
        Task UpdateFidoCredentialAsync(FidoCredential fidoCredential);
        Task DeleteFidoCredentialAsync(Guid userId);
    }
}
