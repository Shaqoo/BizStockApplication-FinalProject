using Application.Commands.PurchaseOrders.RejectPurchaseOrder;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.PurchaseOrders
{
    public class RejectPurchaseOrderCommandHandler : IRequestHandler<RejectPurchaseOrderCommand, Result<bool>>
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RejectPurchaseOrderCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IAuthService _authService;

        public RejectPurchaseOrderCommandHandler(
            IPurchaseOrderRepository purchaseOrderRepository,
            IAuthService authService,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            ILogger<RejectPurchaseOrderCommandHandler> logger,
            IMediator mediator)
        {
            _authService = authService;
            _auditLogRepository = auditLogRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(RejectPurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                _logger.LogWarning("Purchase order {PurchaseOrderId} not found", request.PurchaseOrderId);
                return Result<bool>.Failure("Purchase order not found.");
            }

            if (purchaseOrder.Status == PurchaseOrderStatus.Received ||
                purchaseOrder.Status == PurchaseOrderStatus.Cancelled)
            {
                return Result<bool>.Failure("Purchase order cannot be rejected in its current state.");
            }

            purchaseOrder.Reject(request.RejectPurchaseOrderDto.Reason);
            
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation(
                "Purchase order {PurchaseOrderId} rejected. Reason: {Reason}",
                purchaseOrder.Id,
                request.RejectPurchaseOrderDto.Reason ?? "N/A");

            await _auditLogRepository.AddAsync(new Domain.Entities.AuditLog(
                _authService.CurrentUser()!.Id,
                "RejectPurchaseOrder",
                nameof(Domain.Entities.PurchaseOrder),
                purchaseOrder.Id,
                $"Rejected purchase order {purchaseOrder.OrderNumber}. Reason: {request.RejectPurchaseOrderDto.Reason}",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

            await _mediator.Publish(new PurchaseOrderRejectedEvent(
                purchaseOrder.Id,
                purchaseOrder.OrderNumber,
                purchaseOrder.SupplierId,
                request.RejectPurchaseOrderDto.Reason
            ), cancellationToken);

            return Result<bool>.Success(true);
        }
    }
}
