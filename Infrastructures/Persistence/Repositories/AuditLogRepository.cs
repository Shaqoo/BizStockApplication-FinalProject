using Application.Dto;
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
                    .Fuzziness(Fuzziness.Auto)

                ))
                .From((pageRequest.Page - 1) * pageRequest.PageSize)
                .Size(pageRequest.PageSize)
                .Sort(ss => ss.Descending(f => f.Timestamp))
            );


            return response.IsValid
                ? new PaginatedList<AuditLog>(response.Documents, (int)response.Total, pageRequest.Page, pageRequest.PageSize)
                : new PaginatedList<AuditLog>();
        }

        public async Task<LoginHeatmapDto> GetLoginHeatMap()
        {
            var now = DateTime.UtcNow;
            var fromDate = now.AddDays(-6).Date;  

            var response = await elasticClient.SearchAsync<AuditLog>(s => s
                .Index("audit-logs")
                .Size(0)  
                .Query(q => q
                    .DateRange(r => r
                        .Field(f => f.Timestamp)
                        .GreaterThanOrEquals(fromDate)
                        .LessThanOrEquals(now)
                    ) && q
                    .Bool(b => b
                        .Should(
                            sh => sh.MatchPhrase(m => m.Field(f => f.Action).Query("LOGIN_ATTEMPT_SUCCESS")),
                            sh => sh.MatchPhrase(m => m.Field(f => f.Action).Query("RecoveryLoginSuccess"))
                        )
                        .MinimumShouldMatch(1)
                    )
                )
                .Aggregations(a => a
                    .DateHistogram("per_day", dh => dh
                        .Field(f => f.Timestamp)
                        .CalendarInterval(DateInterval.Day)
                        .MinimumDocumentCount(0)  
                        .ExtendedBounds(fromDate, now)  
                        .Aggregations(aa => aa
                            .Terms("per_hour", t => t
                                .Script("doc['timestamp'].value.getHour()")
                                .Size(24)
                            )
                        )
                    )
                )
            );

            if (!response.IsValid)
                throw new Exception("Elasticsearch query failed: " + response.ServerError?.ToString());

           
            var labels = Enumerable.Range(0, 7)
            .Select(i => now.AddDays(-6 + i).ToString("ddd"))
            .ToList();

            var dayMap = Enumerable.Range(0, 7)
                .ToDictionary(i => now.AddDays(-6 + i).Date, i => new { M = 0, A = 0, E = 0, N = 0 });

            foreach (var day in response.Aggregations.DateHistogram("per_day").Buckets)
            {
                var dayKey = day.Date.Date;
                if (!dayMap.ContainsKey(dayKey)) continue;

                int m = 0, a = 0, e = 0, n = 0;

                foreach (var hourBucket in day.Terms("per_hour").Buckets)
                {
                    int hour = int.Parse(hourBucket.Key);
                    int count = (int)hourBucket.DocCount;

                    if (hour >= 6 && hour < 12) m += count;
                    else if (hour >= 12 && hour < 18) a += count;
                    else if (hour >= 18 && hour < 24) e += count;
                    else n += count;
                }

                dayMap[dayKey] = new { M = m, A = a, E = e, N = n };
            }

            
            var morning = dayMap.Values.Select(v => v.M).ToList();
            var afternoon = dayMap.Values.Select(v => v.A).ToList();
            var evening = dayMap.Values.Select(v => v.E).ToList();
            var night = dayMap.Values.Select(v => v.N).ToList();

            var datasets = new List<LoginHeatmapDatasetDto>
            {
                new("Morning (6-12)", morning, "#3b82f6"),
                new("Afternoon (12-18)", afternoon, "#10b981"),
                new("Evening (18-24)", evening, "#f59e0b"),
                new("Night (0-6)", night, "#ef4444")
            };

            return new LoginHeatmapDto(labels, datasets);

        }

    }
}
