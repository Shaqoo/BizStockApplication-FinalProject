using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;

namespace Application.Queries.AuditLogs.GetLoginHeatMap
{
    public class GetLoginHeatMapHandler(
        IAuditLogRepository auditLogRepository) : IRequestHandler<GetLoginHeatMapQuery, Result<LoginHeatmapDto>>
    {
        public async Task<Result<LoginHeatmapDto>> Handle(GetLoginHeatMapQuery request, CancellationToken cancellationToken)
        {
            var result = await auditLogRepository.GetLoginHeatMap();
            return Result<LoginHeatmapDto>.Success(result);
        }
    }
}
