using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using Domain.Entities;
using MediatR;

namespace Application.Queries.AuditLogs.GetLogsByAction
{
    public class GetAuditLogsByActionQueryHandler
    : IRequestHandler<GetAuditLogsByActionQuery, PaginatedList<AuditLogReadDto>>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IMemoryCacheService _cache;
        private readonly IUserRepository _userRepository;

        public GetAuditLogsByActionQueryHandler(IAuditLogRepository auditLogRepository, IMemoryCacheService cache, IUserRepository userRepository)
        {
            _auditLogRepository = auditLogRepository;
            _cache = cache;
            _userRepository = userRepository;
        }

        public async Task<PaginatedList<AuditLogReadDto>> Handle(GetAuditLogsByActionQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"auditlogs_action_{request.Action}_{request.PageRequest.Page}_{request.PageRequest.PageSize}";

            var result = await _cache.GetOrAddAsync(
                cacheKey,
                async () =>
                {
                    var logs = await _auditLogRepository.GetByActionAsync(request.Action, request.PageRequest);

                    var list = new List<AuditLogReadDto>();
                    foreach (var item in logs.Items)
                    {
                        var user = await _userRepository.GetByIdAsync(item.UserId);
                        if (user == null)
                            break;
                        list.Add(new AuditLogReadDto
                        {
                            Id = item.Id,
                            Action = item.Action,
                            Description = item.Description,
                            Email = (string)user.Email,
                            UserId = item.UserId,
                            EntityId = item.UserId,
                            EntityName = item.EntityName,
                            Fullname = user.FullName,
                            IpAddress = item.IpAddress,
                            ProfilePic = user.ProfilePictureUrl,
                            Timestamp = item.Timestamp,
                            UserAgent = item.UserAgent
                        });
                    }

                    return new PaginatedList<AuditLogReadDto>(
                        list,
                        logs.TotalCount,
                        logs.PageNumber,
                        logs.PageSize
                    );
                },
                TimeSpan.FromMinutes(10)
            );
            return result;
        }
    }

}
