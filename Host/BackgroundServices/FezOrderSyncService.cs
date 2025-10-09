using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Host.BackgroundServices
{
    public class FezOrderSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FezOrderSyncService> _logger;

        public FezOrderSyncService(IServiceProvider serviceProvider, ILogger<FezOrderSyncService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var fezService = scope.ServiceProvider.GetRequiredService<IFezService>();
                    var repo = scope.ServiceProvider.GetRequiredService<ISalesOrderItemRepository>();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var pendingItems = await repo.FindAsync(a => a.FezOrderNo == null);

                    if (!pendingItems.Any())
                    {
                        _logger.LogInformation("No pending Fez order items found. Waiting...");
                        await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
                        continue;
                    }

                    // Convert DateTimeOffset to UTC DateTime for Fez API
                    var minDate = pendingItems.Min(x => x.DateCreated.UtcDateTime).AddMinutes(-5);
                    var maxDate = pendingItems.Max(x => x.DateCreated.UtcDateTime).AddMinutes(5);

                    _logger.LogInformation("Fetching Fez orders between {Start} and {End}", minDate, maxDate);

                    var result = await fezService.GetOrdersByStatusAsync(minDate, maxDate);
                    if (!result.Success || result.Data == null)
                    {
                        _logger.LogWarning("Failed to fetch Fez orders: {msg}", result.Message);
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                        continue;
                    }

                    var fezOrders = result.Data;
                    int matchCount = 0;

                   
                    var grouped = pendingItems
                        .GroupBy(x => x.DateCreated.UtcDateTime.ToString("yyyy-MM-dd HH:mm"))
                        .ToList();

                    foreach (var group in grouped)
                    {
                        var groupKey = group.Key;
                        var groupItems = group.OrderBy(x => x.Id).ToList();

                        var fezMatches = fezOrders
                            .Where(fz =>
                            {
                                if (DateTime.TryParse(fz.OrderDate, out var fezDate))
                                {
                                    var fezUtc = fezDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm");
                                    return fezUtc == groupKey;
                                }
                                return false;
                            })
                            .OrderBy(fz => DateTime.Parse(fz.OrderDate!).ToUniversalTime())
                            .ToList();

                        if (!fezMatches.Any())
                        {
                            _logger.LogInformation("No Fez match found for group {GroupKey}", groupKey);
                            continue;
                        }

                        for (int i = 0; i < Math.Min(groupItems.Count, fezMatches.Count); i++)
                        {
                            var item = groupItems[i];
                            var fezOrder = fezMatches[i];

                            item.UpdateFezOrderNo(fezOrder.OrderNo);
                            _logger.LogInformation("Matched item {UniqueId} with Fez order {OrderNo}", item.UniqueId, fezOrder.OrderNo);
                            matchCount++;
                        }
                    }

                    if (matchCount > 0)
                    {
                        await uow.SaveChangesAsync();
                        _logger.LogInformation("✅ Successfully updated {Count} items with Fez order numbers.", matchCount);
                    }
                    else
                    {
                        _logger.LogInformation("No Fez order matches found for this cycle.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while syncing Fez orders");
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
