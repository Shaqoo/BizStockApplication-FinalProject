using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.Users.GetLostAccessRequests
{
    public class GetPendingUserLostAccessRequestHandler(ILostAccessRequestRepository lostAccessRequestRepository,
        IMemoryCacheService memoryCacheService)
        : IRequestHandler<GetPendingUserLostAccessRequestsQuery, Result<PaginatedList<LostAccessRequestDto>>>
    {
        public async Task<Result<PaginatedList<LostAccessRequestDto>>> Handle(GetPendingUserLostAccessRequestsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"PendingLostAccessRequests-{request.PageRequest.Page}-{request.PageRequest.PageSize}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey,
                async () =>
                {
                    var requests = await lostAccessRequestRepository.GetPendingRequestsAsync(request.PageRequest);
                    var requestDtos = requests.Items.Select(r => new LostAccessRequestDto
                    {
                        Id = r.Id,
                        UserIdentifier = r.UserIdentifier,
                        Status = r.Status,
                        SubmittedAt = r.SubmittedAt,
                        AdminNotes = r.AdminNotes,
                        AlternateEmail = r.AlternateEmail,
                        AlternatePhone = r.AlternatePhone,
                        ProblemDescription = r.ProblemDescription
                    }).ToList();
                    return new PaginatedList<LostAccessRequestDto>(requestDtos, requests.TotalCount, request.PageRequest.Page, request.PageRequest.PageSize);
                },TimeSpan.FromMinutes(5));

            return Result<PaginatedList<LostAccessRequestDto>>.Success(cachedResult);
        }
    }
}
