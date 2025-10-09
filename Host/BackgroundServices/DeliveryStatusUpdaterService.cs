using Application.Configurations;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Enums;
using MassTransit;
using MediatR;

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
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                    var itemsToCheck = await salesOrderItemRepo.GetPendingOrInTransitAsync();

                    foreach (var item in itemsToCheck)
                    {
                        if (string.IsNullOrEmpty(item.FezOrderNo))
                            continue;

                        var response = await fezService.TrackOrderAsync(item.FezOrderNo!);
                        if (response.Success && response.Data != null)
                        {
                            if (response.Data.History == null || !response.Data.History.Any())
                            {
                                FezHelper.UpdateOrderHistory(response.Data);
                            }

                            var fezStatus = response.Data.History?.LastOrDefault();
                            if (fezStatus == null)
                                continue;

                            var mappedStatus = MapFezStatusToLocal(fezStatus.OrderStatus);
                            if (item.DeliveryStatus != mappedStatus)
                            {
                                var @event = new OrderStatusChangedEvent(item.SalesOrderId, item.DeliveryStatus.ToString()
                                    , mappedStatus.ToString(), item.FezOrderNo, item.SalesOrder.Customer.Email.Value, item.SalesOrder.Customer.FullName, GetOrderStatusMessage(mappedStatus));
                                item.UpdateDeliveryStatus(mappedStatus);

                                await mediator.Publish(@event, stoppingToken);
                                //await publishEndpoint.Publish(@event, stoppingToken);
                                await salesOrderItemRepo.UpdateAsync(item);
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
            if (string.IsNullOrWhiteSpace(fezStatus))
                return DeliveryStatus.Pending;

            return fezStatus.Trim().ToLower() switch
            {
                "pending" or "pending pickup" => DeliveryStatus.Pending,
                "picked up" or "picked up" => DeliveryStatus.Processing,
                "dispatched" or "intransit" or "in transit" => DeliveryStatus.InTransit,
                "out for delivery" => DeliveryStatus.OutForDelivery,
                "delivered" => DeliveryStatus.Delivered,
                "failed" or "cancelled" or "returned" => DeliveryStatus.Failed,
                _ => DeliveryStatus.Pending
            };
        }

        private static string GetOrderStatusMessage(DeliveryStatus status)
        {
            return status switch
            {
                DeliveryStatus.Pending => "We’ve received your order and it’s being prepared for pickup.",
                DeliveryStatus.Processing => "Your order is being processed and will soon be handed over to the dispatch team.",
                DeliveryStatus.InTransit => "Your package is now on its way! You can track its journey in real time.",
                DeliveryStatus.OutForDelivery => "Your package is out for delivery today. Please ensure someone is available to receive it.",
                DeliveryStatus.Delivered => "Your package has been successfully delivered. We hope you love your purchase!",
                DeliveryStatus.Failed => "Unfortunately, the delivery could not be completed. Please contact our support team for assistance.",
                _ => "We’re updating your order status. Stay tuned for the latest update."
            };
        }


    }
}

