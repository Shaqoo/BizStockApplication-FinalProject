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
             var response = await elasticClient.IndexDocumentAsync(auditLog);
            return response.IsValid;
        }

        public async Task<PaginatedList<AuditLog>> GetAllAsync(PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
           .From((pageRequest.Page - 1) * pageRequest.PageSize)
           .Size(pageRequest.PageSize)
           .Sort(ss => ss.Descending(f => f.Timestamp))
           );
            return response.IsValid ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize) : new PaginatedList<AuditLog>();
        }

        public async Task<PaginatedList<AuditLog>> GetByActionAsync(string action, PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
               .Index("audit-logs")
               .Query(q => q
                   .Term(t => t.Action, action)
               )
               .From((pageRequest.Page - 1) * pageRequest.PageSize)
               .Size(pageRequest.PageSize)
               .Sort(ss => ss.Descending(f => f.Timestamp)));

            return response.IsValid ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize) : new PaginatedList<AuditLog>();
        }

        public async Task<PaginatedList<AuditLog>> GetByUserId(Guid userId, PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
              .Index("audit-logs")
              .Query(q => q
                  .Term(t => t.UserId, userId)
              )
              .From((pageRequest.Page - 1) * pageRequest.PageSize)
              .Size(pageRequest.PageSize)
              .Sort(ss => ss.Descending(f => f.Timestamp)));

            return response.IsValid ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize) : new PaginatedList<AuditLog>();
        }

        public async Task<PaginatedList<AuditLog>> SearchAsync(string search, PageRequest pageRequest)
        {
            var response = await elasticClient.SearchAsync<AuditLog>(s => s
               .Index("audit-logs")
               .Query(q => q
                .MultiMatch(m => m
                 .Fields(f => f
                  .Field(ff => ff.Action)
                  .Field(ff => ff.Description)
                  .Field(ff => ff.UserAgent)
                 )
                 .Query(search)
                 .Fuzziness(Fuzziness.Auto)
                )
               )
               .From((pageRequest.Page - 1) * pageRequest.PageSize)
               .Size(pageRequest.PageSize)
               .Sort(ss => ss.Descending(f => f.Timestamp))
            );
            return response.IsValid ? new PaginatedList<AuditLog>(response.Documents,(int)response.Total,pageRequest.Page,pageRequest.PageSize) : new PaginatedList<AuditLog>();
        }
    }
}
