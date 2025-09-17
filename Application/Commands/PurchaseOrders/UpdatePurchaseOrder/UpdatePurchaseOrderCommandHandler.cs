using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.PurchaseOrders.UpdatePurchaseOrder
{
    public class UpdatePurchaseOrderCommandHandler(
    IPurchaseOrderRepository purchaseOrderRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditLogRepository auditLogRepository,
    ILogger<UpdatePurchaseOrderCommandHandler> logger,
    IMediator mediator
) : IRequestHandler<UpdatePurchaseOrderCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.Dto;

                var purchaseOrder = await purchaseOrderRepository.GetByIdAsync(dto.PurchaseOrderId);
                if (purchaseOrder == null)
                    return Result<Guid>.Failure("Purchase order not found.");

                if(purchaseOrder.Status != PurchaseOrderStatus.Draft)
                    return Result<Guid>.Failure("Only purchase orders in Draft status can be updated.");

                purchaseOrder.Update(dto.Discount, dto.Tax, dto.Notes);

                await unitOfWork.SaveChangesAsync();

                var user = authService.CurrentUser();
                await auditLogRepository.AddAsync(new AuditLog(
                    user!.Id,
                    "UpdatePurchaseOrder",
                    nameof(PurchaseOrder),
                    purchaseOrder.Id,
                    $"Updated PO {purchaseOrder.OrderNumber} (discount: {dto.Discount}, tax: {dto.Tax})",
                    request.Metadata.IpAddress,
                    request.Metadata.UserAgent
                ));

                await mediator.Publish(new PurchaseOrderUpdatedEvent(purchaseOrder.Id,purchaseOrder.OrderNumber,purchaseOrder.SupplierId, dto.Notes,dto.Discount,dto.Tax));

                logger.LogInformation("Purchase order {PO} updated successfully.", purchaseOrder.OrderNumber);

                return Result<Guid>.Success(purchaseOrder.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating purchase order.");
                return Result<Guid>.Failure("An error occurred while updating the purchase order.");
            }
        }
    }

}
