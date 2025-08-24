using Application.Interfaces.Repository;
using Application.Pagination;
using Domain.Entities;
using Nest;

namespace Infrastructures.Persistence.Repositories
{
    public class AuditLogRepository(IElasticClient elasticClient) : IAuditLogRepository
    {
        public async Task<bool> AddAsync(AuditLog auditLog)
        {
            var response = await elasticClient.IndexAsync(auditLog, i => i.Index("audit-logs"));
            return response.IsValid;
        }

        public async Task<PaginatedList<AuditLog>> GetAllAsync(PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
                .Index("audit-logs")
                .From((pageRequest.Page - 1) * pageRequest.PageSize)
                .Size(pageRequest.PageSize)
                .Sort(ss => ss.Descending(f => f.Timestamp))
            );

            return response.IsValid
                ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize)
                : new PaginatedList<AuditLog>();
        }

        public async Task<PaginatedList<AuditLog>> GetByActionAsync(string action, PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
                .Index("audit-logs")
                .Query(q => q
                    .Term(t => t
                        .Field(f => f.Action.Suffix("keyword"))  
                        .Value(action)
                    )
                )
                .From((pageRequest.Page - 1) * pageRequest.PageSize)
                .Size(pageRequest.PageSize)
                .Sort(ss => ss.Descending(f => f.Timestamp))
            );


            return response.IsValid
                ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize)
                : new PaginatedList<AuditLog>();
        }

        public async Task<PaginatedList<AuditLog>> GetByUserId(Guid userId, PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
                 .Index("audit-logs")
                 .Query(q => q
                     .Term(t => t
                         .Field(f => f.UserId.Suffix("keyword"))  
                         .Value(userId.ToString())
                     )
                 )
                 .From((pageRequest.Page - 1) * pageRequest.PageSize)
                 .Size(pageRequest.PageSize)
                 .Sort(ss => ss.Descending(f => f.Timestamp))
             );


            return response.IsValid
                ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize)
                : new PaginatedList<AuditLog>();
        }

        public async Task<PaginatedList<AuditLog>> SearchAsync(string search, PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
                .Index("audit-logs")
                .Query(q => q.MultiMatch(m => m
                    .Fields(f => f.Field(ff => ff.Action).Field(ff => ff.Description).Field(ff => ff.UserAgent))
                    .Query(search)
                     
                ))
                .From((pageRequest.Page - 1) * pageRequest.PageSize)
                .Size(pageRequest.PageSize)
                .Sort(ss => ss.Descending(f => f.Timestamp))
            );


            return response.IsValid
                ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize)
                : new PaginatedList<AuditLog>();
        }
    }
}
