using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Refunds.ProcessRefund
{
    public class ProcessRefundCommandHandler : IRequestHandler<ProcessRefundCommand, Result<Guid>>
    {
        private readonly IRefundRepository _refundRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IPaymentGatewayService _paymentGateway;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProcessRefundCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IAuditLogRepository _auditLogRepository;   
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        private readonly IPaymentRepository _paymentRepository;

        public ProcessRefundCommandHandler(
            IRefundRepository refundRepository,
            IWalletRepository walletRepository,
            IPaymentRepository paymentRepository,
            IMediator mediator,
            IAuditLogRepository auditLogRepository,
            IWalletTransactionRepository walletTransactionRepository,
            ISalesOrderRepository salesOrderRepository,
            IPaymentGatewayService paymentGateway,
            IUnitOfWork unitOfWork,
            ILogger<ProcessRefundCommandHandler> logger)
        {
            _mediator = mediator;
            _auditLogRepository = auditLogRepository;
            _paymentRepository = paymentRepository;
            _refundRepository = refundRepository;
            _walletRepository = walletRepository;
            _walletTransactionRepository = walletTransactionRepository;
            _salesOrderRepository = salesOrderRepository;
            _paymentGateway = paymentGateway;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(ProcessRefundCommand request, CancellationToken cancellationToken)
        {
            var order = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            if (order == null)
                return Result<Guid>.Failure("Order not found");

            var refund = new Refund(
                order.Id,
                request.Amount,
                request.PaymentMethod,
                transactionRef: request.ReferenceNo,
                reason: request.Reason
            );

            await _refundRepository.AddAsync(refund);
           
            try
            {
                string refundReference = "";

                switch (request.PaymentMethod)
                {
                    case PaymentMethod.Online:
                        var paystackResponse = await _paymentGateway.RefundTransactionAsync(
                            refund.TransactionReference,
                            refund.Amount,
                            refund.Reason
                        );
                        refundReference = paystackResponse.RefundReference;
                        refund.MarkCompleted(refundReference);
                        break;

                    case PaymentMethod.Wallet:
                        var wallet = await _walletRepository.GetByUserIdAsync(order.CustomerId);
                        if (wallet == null)
                        {
                            _logger.LogWarning("Wallet not found for Customer {CustomerId}", order.CustomerId);
                            return Result<Guid>.Failure("Wallet not found");
                        }
                        wallet.Credit(request.Amount);
                        _logger.LogInformation("Crediting wallet {WalletId} with amount {Amount}", wallet.Id, request.Amount);
                        var reference = $"PAY-{Guid.NewGuid():N}";
                        var payment = new Payment(
                            reference,
                            order.CustomerId,
                            request.Amount,
                            PaymentMethod.Wallet,
                            PaymentPurpose.WalletFunding,
                            null, null,
                            $"Payment for {PaymentPurpose.WalletFunding}"
                        );

                        var walletTransaction = new WalletTransaction(
                            wallet.Id,
                            request.Amount,
                            TransactionType.Credit,
                            refund.TransactionReference,
                            payment.Id,
                            refund.Reason
                        );
                        _logger.LogInformation("Creating wallet transaction for refund: {WalletTransaction}", walletTransaction);
                        payment.MarkAsCompleted();
                        await _paymentRepository.AddAsync(payment);
                        await _walletTransactionRepository.AddAsync(walletTransaction);
                        refund.MarkCompleted("WalletRefund_" + Guid.NewGuid());
                        break;

                    default:
                        refund.MarkFailed("Unsupported refund method");
                        break;
                }

                order.MarkAsCancelled();
                _logger.LogInformation("Marking order {OrderId} as cancelled", order.Id);
                order.Invoice.MarkAsCancelled();

               // await _refundRepository.UpdateAsync(refund);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

               _logger.LogInformation("Refund processed successfully for Order {OrderId}", order.Id);

                await _auditLogRepository.AddAsync(new AuditLog(
                    userId: order.CustomerId,
                    action: "Refund Processed",
                    entityName: "Refund",
                    entityId: refund.Id,
                    details: $"Refund of ₦{refund.Amount:N2} for Order {order.Id} was successful. Reference: {refundReference}",
                    ip: request.RequestMetadata.IpAddress,
                    userAgent: request.RequestMetadata.UserAgent
                ));

                await _mediator.Publish(new RefundProcessedEvent(
                    refund.Id,
                    order.Id,
                    order.OrderNumber,
                    refund.Amount,
                    refund.PaymentMethod.ToString(),
                    refundReference,
                    refund.Status.ToString(),
                    refund.Reason,
                    order.CustomerId,
                    DateTime.UtcNow
                ), cancellationToken);
                return Result<Guid>.Success(refund.Id);
            }
            catch (Exception ex)
            {
                refund.MarkFailed(ex.Message);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _auditLogRepository.AddAsync(new AuditLog(
                    userId: order.CustomerId,
                    action: "Refund Failed",
                    entityName: "Refund",
                    entityId: refund.Id,
                    details: $"Refund of ₦{refund.Amount:N2} for Order {order.Id} failed. Reason: {ex.Message}",
                    ip: request.RequestMetadata.IpAddress,
                    userAgent: request.RequestMetadata.UserAgent
                ));

                _logger.LogError(ex, "Refund failed for Order {OrderId}", order.Id);
                return Result<Guid>.Failure("Refund processing failed");
            }
        }
    }
}
