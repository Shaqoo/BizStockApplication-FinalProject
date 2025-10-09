using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructures.Persistence.Repositories
{
    public class FidoCredentialRepository : IFidoCredentialRepository
    {
        private readonly BizStockContext _context;

        public FidoCredentialRepository(BizStockContext context)
        {
            _context = context;
        }

        public async Task AddAsync(FidoCredential credential)
        {
            await _context.FidoCredentials.AddAsync(credential);
        }

        public async Task<FidoCredential?> GetByIdAsync(Guid id)
        {
            return await _context.FidoCredentials.FindAsync(id)
                ?? throw new KeyNotFoundException("FIDO credential not found.");
        }

        public async Task<PaginatedList<FidoCredential>> GetAllAsync(PageRequest pageRequest)
        {
            var query = _context.FidoCredentials.AsQueryable();
            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageRequest.Page - 1) * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<FidoCredential>(items, total, pageRequest.Page, pageRequest.PageSize);
        }

        public async Task<IEnumerable<FidoCredential>> FindAsync(Expression<Func<FidoCredential, bool>> predicate)
        {
            return await _context.FidoCredentials.Where(predicate).ToListAsync();
        }

        public async Task<List<FidoCredential>> GetByUserIdAsync(Guid userId)
        {
            return await _context.FidoCredentials
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<FidoCredential?> GetByCredentialIdAsync(string credentialId)
        {
            return await _context.FidoCredentials.Include(a => a.User)
                .FirstOrDefaultAsync(f => f.CredentialId == credentialId);
        }



        public async Task UpdateFidoCredentialAsync(FidoCredential fidoCredential)
        {
            _context.FidoCredentials.Update(fidoCredential);
            await Task.CompletedTask;
        }

        public async Task DeleteFidoCredentialAsync(Guid userId)
        {
            var credentials = await _context.FidoCredentials
                .Where(f => f.UserId == userId)
                .ToListAsync();

            if (credentials.Any())
            {
                _context.FidoCredentials.RemoveRange(credentials);
            }

            await Task.CompletedTask;
        }

        public async Task<FidoCredential?> GetByExpression(Expression<Func<FidoCredential, bool>> predicate)
        {
            return await _context.FidoCredentials.FirstOrDefaultAsync(predicate) ??

               throw new ArgumentNullException("Fido Credential Not Found");
        }
    }

}

