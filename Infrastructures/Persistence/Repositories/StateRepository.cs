using Application.Interfaces.Repository;
using Domain.Entities;
using Infrastructures.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructures.Persistence.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly BizStockContext _context;
        public StateRepository(BizStockContext context) => _context = context;

        public async Task<State?> GetByIdAsync(int id) =>
            await _context.States
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<IEnumerable<State>> GetAllAsync() =>
            await _context.States
                .ToListAsync();
    }

}
