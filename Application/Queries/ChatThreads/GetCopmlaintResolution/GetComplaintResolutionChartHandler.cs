using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.ChatThreads.GetCopmlaintResolution
{
    public class GetComplaintResolutionChartHandler(IMemoryCacheService memoryCacheService,
        IChatThreadRepository chatThreadRepository) : IRequestHandler<GetComplaintResolutionChartQuery, Result<ComplaintResolutionChartDto>>
    {
        public async Task<Result<ComplaintResolutionChartDto>> Handle(GetComplaintResolutionChartQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "GetComplaintResolutionChartQuery";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var dto = await chatThreadRepository.GetResolutionChartDto();
                return dto;
            },TimeSpan.FromMinutes(10));

            return Result<ComplaintResolutionChartDto>.Success(cachedResult ?? new ComplaintResolutionChartDto());
        }
    }
}
