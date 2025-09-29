using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Lgas.GetLgaById
{
    public class GetLgaByIdHandler : IRequestHandler<GetLgaByIdQuery, Lga?>
    {
        private readonly ILgaRepository _lgaRepo;
        private readonly IMemoryCacheService _cache;

        public GetLgaByIdHandler(ILgaRepository lgaRepo, IMemoryCacheService cache)
        {
            _lgaRepo = lgaRepo;
            _cache = cache;
        }

        public async Task<Lga?> Handle(GetLgaByIdQuery request, CancellationToken cancellationToken)
        {
            return await _cache.GetOrAddAsync(
                $"lgas:{request.LgaId}",
                async () => await _lgaRepo.GetByIdAsync(request.LgaId),
                TimeSpan.FromHours(2)
            );
        }
    }
}
