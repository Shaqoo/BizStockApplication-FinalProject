using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructures.Persistence.Repositories
{
    public class LgaRepository : ILgaRepository
    {
        private readonly BizStockContext _context;
        public LgaRepository(BizStockContext context) => _context = context;

        public async Task<Lga?> GetByIdAsync(int id) =>
            await _context.Lgas

                .FirstOrDefaultAsync(l => l.Id == id);

        public async Task<IEnumerable<Lga>> GetByStateIdAsync(int stateId) =>
            await _context.Lgas
                .Where(l => l.StateId == stateId)
                .ToListAsync();
    }
}
