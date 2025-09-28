using Application.Dto;
using MediatR;

namespace Application.Queries.AuditLogs.GetLoginHeatMap
{
    public record GetLoginHeatMapQuery() : IRequest<Result<LoginHeatmapDto>>;
    
}
