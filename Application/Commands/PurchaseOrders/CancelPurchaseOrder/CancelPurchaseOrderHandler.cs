using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.CancelPurchaseOrder
{
    public class CancelPurchaseOrderHandler(
        IPurchaseOrderRepository purchaseOrderRepository,
        IUnitOfWork unitOfWork,
        IAuthService authService,
        IAuditLogRepository auditLogRepository,
        IMediator mediator,
        ILogger<CancelPurchaseOrderHandler> logger
    ) : IRequestHandler<CancelPurchaseOrderCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(CancelPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var po = await purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
                if (po == null)
                {
                    logger.LogWarning("Purchase order {PurchaseOrderId} not found", request.PurchaseOrderId);
                    return Result<bool>.Failure("Purchase order not found.");
                }

                if (po.Status == PurchaseOrderStatus.Received || po.Status == PurchaseOrderStatus.PartiallyReceived)
                {
                    logger.LogWarning("Purchase order {PurchaseOrderId} cannot be cancelled from status {Status}", po.Id, po.Status);
                    return Result<bool>.Failure("Purchase order cannot be cancelled in its current status.");
                }

                po.Cancel();
                po.AddNotes($"{po.Notes}\n[Cancelled: {request.CancelPurchaseOrderDto.Reason}]");

                await unitOfWork.BeginTransactionAsync();

                await unitOfWork.CommitTransactionAsync();

                var user = authService.CurrentUser();
                var audit = new AuditLog(
                    user!.Id,
                    "CancelPurchaseOrder",
                    nameof(PurchaseOrder),
                    po.Id,
                    $"Cancelled purchase order {po.OrderNumber}. Reason: {request.CancelPurchaseOrderDto.Reason}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );
                await auditLogRepository.AddAsync(audit);

                var evt = new PurchaseOrderCancelledEvent(
                    po.Id,
                    po.OrderNumber,
                    po.SupplierId,
                    request.CancelPurchaseOrderDto.Reason
                );
                await mediator.Publish(evt, cancellationToken);

                logger.LogInformation("Purchase order {PurchaseOrderId} cancelled successfully", po.Id);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                logger.LogError(ex, "Error cancelling purchase order {PurchaseOrderId}", request.PurchaseOrderId);
                return Result<bool>.Failure("An error occurred while cancelling the purchase order.");
            }
        }
    }
}
