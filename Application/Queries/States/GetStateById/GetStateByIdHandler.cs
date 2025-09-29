using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Entities;
using MediatR;

namespace Application.Queries.States.GetStateById
{
    public class GetStateByIdHandler : IRequestHandler<GetStateByIdQuery, State?>
    {
        private readonly IStateRepository _stateRepo;
        private readonly IMemoryCacheService _cache;

        public GetStateByIdHandler(IStateRepository stateRepo, IMemoryCacheService cache)
        {
            _stateRepo = stateRepo;
            _cache = cache;
        }

        public async Task<State?> Handle(GetStateByIdQuery request, CancellationToken cancellationToken)
        {
            return await _cache.GetOrAddAsync(
                $"states:{request.StateId}",
                async () => await _stateRepo.GetByIdAsync(request.StateId),
                TimeSpan.FromHours(2)
            );
        }
    }

}
