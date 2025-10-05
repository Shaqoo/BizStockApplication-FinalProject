using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Pagination;
using MediatR;

namespace Application.Queries.DeliveryAssignments.GetAll
{
    public class GetDeliveryAssignmentsQueryHandler(IDeliveryAssignmentRepository deliveryAssignmentRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<GetDeliveryAssignmentsQuery, Result<PaginatedList<DeliveryAssignmentDto>>>
    {
        public async Task<Result<PaginatedList<DeliveryAssignmentDto>>> Handle(GetDeliveryAssignmentsQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"DeliveryAssignments_Page_{request.PageRequest.Page}_Size_{request.PageRequest.PageSize}";

            var cachedResult = await memoryCacheService.GetOrAddAsync(cacheKey, async () =>
            {
                var assignments = await deliveryAssignmentRepository.GetAllAsync(request.PageRequest);
                var assignmentDtos = assignments.Items.Select(da => new DeliveryAssignmentDto
                {
                    Id = da.Id,
                    DeliveredAt = da.DeliveredAt,
                    DeliveryAgentId = da.DeliveryAgentId,
                    DeliveryAgentName = da.DeliveryAgent?.FullName ?? "",
                    Status = da.Status,
                    DeliveryAgentPhone = da.DeliveryAgent?.ContactNumber ?? "",
                    DeliveryFee = da.DeliveryFee,
                    ExternalDeliveryService = da.ExternalDeliveryService,
                    ExternalJobId = da.ExternalJobId,
                    IsExternal = da.IsExternal,
                    Note = da.Note,
                    RecipientEmail = da.RecipientEmail,
                    RecipientName = da.RecipientName,
                    RecipientPhone = da.RecipientPhone,
                    SalesOrderId = da.SalesOrderId,
                    DeliveryAddressId = da.DeliveryAddressId
                }).ToList();
                 var paginatedListDto = new PaginatedList<DeliveryAssignmentDto>(assignmentDtos, assignments.TotalCount, assignments.PageNumber, assignments.PageSize);
                return Result<PaginatedList<DeliveryAssignmentDto>>.Success(paginatedListDto);
            }, TimeSpan.FromMinutes(10));

            return cachedResult ?? Result<PaginatedList<DeliveryAssignmentDto>>.Success(new PaginatedList<DeliveryAssignmentDto>(new List<DeliveryAssignmentDto>(), 0, request.PageRequest.Page, request.PageRequest.PageSize));
        }
    }
}
