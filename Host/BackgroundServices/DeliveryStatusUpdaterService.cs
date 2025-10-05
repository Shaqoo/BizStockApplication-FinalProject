using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Enums;
using MassTransit.Util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Host.BackgroundServices
{
    
    public class DeliveryStatusUpdaterService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DeliveryStatusUpdaterService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(10);  

        public DeliveryStatusUpdaterService(IServiceProvider serviceProvider, ILogger<DeliveryStatusUpdaterService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Delivery Status Updater Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var fezService = scope.ServiceProvider.GetRequiredService<IFezService>();
                    var salesOrderItemRepo = scope.ServiceProvider.GetRequiredService<ISalesOrderItemRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var itemsToCheck = await salesOrderItemRepo.GetPendingOrInTransitAsync();

                    foreach (var item in itemsToCheck)
                    {
                        if (string.IsNullOrEmpty(item.UniqueId))
                            continue;

                        var response = await fezService.TrackOrderAsync(item.UniqueId);
                        if (response.Success && response.Data != null)
                        {
                            var fezStatus = response.Data?.History.LastOrDefault();
                            if (fezStatus == null)
                                continue;

                            var mappedStatus = MapFezStatusToLocal(fezStatus.OrderStatus);
                            if (item.DeliveryStatus != mappedStatus)
                            {
                                item.UpdateDeliveryStatus(mappedStatus);
                                _logger.LogInformation("Updated order {OrderNo} to {Status}", item.UniqueId, mappedStatus);
                            }
                        }
                        Console.WriteLine(response);
                    }

                    await unitOfWork.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating delivery statuses.");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("Delivery Status Updater Service stopped.");
        }

        private static DeliveryStatus MapFezStatusToLocal(string fezStatus)
        {
            return fezStatus.ToLower() switch
            {
                "pending" => DeliveryStatus.Pending,
                "processing" => DeliveryStatus.Processing,
                "dispatched" or "intransit" or "picked-up" => DeliveryStatus.InTransit,
                "delivered" => DeliveryStatus.Delivered,
                "failed" or "cancelled" => DeliveryStatus.Failed,
                _ => DeliveryStatus.Pending
            };
        }
    }
}

