using Application.Pagination;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repository
{
    public interface IAuditLogRepository
    {
        Task<PaginatedList<AuditLog>> GetAllAsync(PageRequest pageRequest);
        Task<PaginatedList<AuditLog>> GetByUserId(Guid userId,PageRequest pageRequest);
        Task<PaginatedList<AuditLog>> GetByActionAsync(string action, PageRequest pageRequest);
        Task<PaginatedList<AuditLog>> SearchAsync(string search, PageRequest pageRequest);
        Task<bool> AddAsync(AuditLog auditLog);
    }
}
