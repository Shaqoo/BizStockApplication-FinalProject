using Application.Dto;
using MediatR;

namespace Application.Queries.ChatThreads.GetCopmlaintResolution
{
    public record GetComplaintResolutionChartQuery() : IRequest<Result<ComplaintResolutionChartDto>>;
    
}
