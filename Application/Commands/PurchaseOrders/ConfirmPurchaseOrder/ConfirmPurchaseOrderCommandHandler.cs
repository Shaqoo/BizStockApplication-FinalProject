using Application.Commands.PurchaseOrders;
using Application.Commands.PurchaseOrders.ConfirmPurchaseOrder;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CommandHandlers.PurchaseOrders
{
    public class ConfirmPurchaseOrderCommandHandler : IRequestHandler<ConfirmPurchaseOrderCommand, Result<bool>>
    {
        private readonly ILogger<ConfirmPurchaseOrderCommandHandler> _logger;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IAuthService _authService;
        public ConfirmPurchaseOrderCommandHandler(
            ILogger<ConfirmPurchaseOrderCommandHandler> logger,
            IPurchaseOrderRepository purchaseOrderRepository,
            IAuditLogRepository auditLogRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _authService = authService;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(ConfirmPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
                if (purchaseOrder == null)
                {
                    return Result<bool>.Failure("Purchase order not found.");
                }

                if (purchaseOrder.Status != PurchaseOrderStatus.Draft)
                {
                    return Result<bool>.Failure(
                        $"Purchase order cannot be confirmed because it is in {purchaseOrder.Status} status.");
                }

                if(request.ConfirmPurchaseOrderDto.Notes is not null)
                   purchaseOrder.AddNotes(request.ConfirmPurchaseOrderDto.Notes);

                purchaseOrder.Confirm(request.ConfirmPurchaseOrderDto.ExpectedDeliveryDate);


                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Purchase order {PurchaseOrderId} confirmed", purchaseOrder.Id);

                await _auditLogRepository.AddAsync(new AuditLog(
                    _authService.CurrentUser()!.Id,
                    "ConfirmPurchaseOrder",
                    nameof(PurchaseOrder),
                    purchaseOrder.Id,
                    $"Confirmed purchase order {purchaseOrder.OrderNumber}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                await _mediator.Publish(
                    new PurchaseOrderConfirmedEvent(
                        purchaseOrder.Id,
                        purchaseOrder.OrderNumber,
                        request.ConfirmPurchaseOrderDto.Notes,
                        purchaseOrder.SupplierId),
                    cancellationToken);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error confirming purchase order {PurchaseOrderId}",
                    request.PurchaseOrderId);
                return Result<bool>.Failure("An error occurred while confirming the purchase order.");
            }
        }
    }
}
