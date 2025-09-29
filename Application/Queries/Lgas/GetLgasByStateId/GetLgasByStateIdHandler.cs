using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Lgas.GetLgasByStateId
{
    public class GetLgasByStateIdHandler : IRequestHandler<GetLgasByStateIdQuery, IEnumerable<Lga>>
    {
        private readonly ILgaRepository _lgaRepo;
        private readonly IMemoryCacheService _cache;

        public GetLgasByStateIdHandler(ILgaRepository lgaRepo, IMemoryCacheService cache)
        {
            _lgaRepo = lgaRepo;
            _cache = cache;
        }

        public async Task<IEnumerable<Lga>> Handle(GetLgasByStateIdQuery request, CancellationToken cancellationToken)
        {
            return await _cache.GetOrAddAsync(
                $"lgas:state:{request.StateId}",
                async () => await _lgaRepo.GetByStateIdAsync(request.StateId),
                TimeSpan.FromHours(2)
            );
        }
    }
}
