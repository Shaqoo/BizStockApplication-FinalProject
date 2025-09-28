using Application.Dto;
using Application.Pagination;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IAuditLogRepository
    {
        Task<PaginatedList<AuditLog>> GetAllAsync(PageRequest pageRequest);
        Task<PaginatedList<AuditLog>> GetByUserId(Guid userId,PageRequest pageRequest);
        Task<PaginatedList<AuditLog>> GetByActionAsync(string action, PageRequest pageRequest);
        Task<PaginatedList<AuditLog>> SearchAsync(string search, PageRequest pageRequest);
        Task<LoginHeatmapDto> GetLoginHeatMap();
        Task<bool> AddAsync(AuditLog auditLog);
    }
}
