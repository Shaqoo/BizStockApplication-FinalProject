using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;

namespace Application.Queries.States.GetAllStates
{
    public class GetAllStatesHandler : IRequestHandler<GetAllStatesQuery, IEnumerable<State>>
    {
        private readonly IStateRepository _stateRepo;
        private readonly IMemoryCacheService _cache;

        public GetAllStatesHandler(IStateRepository stateRepo, IMemoryCacheService cache)
        {
            _stateRepo = stateRepo;
            _cache = cache;
        }

        public async Task<IEnumerable<State>> Handle(GetAllStatesQuery request, CancellationToken cancellationToken)
        {
            var result =  await _cache.GetOrAddAsync(
                "states:all",
                async () => await _stateRepo.GetAllAsync(),
                TimeSpan.FromHours(2)
            );
            return result ?? Enumerable.Empty<State>();
        }
    }
}
