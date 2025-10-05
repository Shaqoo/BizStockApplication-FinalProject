using Application.Commands.SalesOrders.Create;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Payments.VerifyPayment
{
    public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, Result<PaystackVerifyResponse>>
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IWalletRepository walletRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentGatewayService paymentGatewayService;
        private readonly IWalletTransactionRepository walletTransactionRepository;
        private readonly ILogger<VerifyPaymentCommandHandler> logger;
        private readonly IMediator mediator;
        private readonly IAuditLogRepository auditLogRepository;
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VerifyPaymentCommandHandler(
            IWalletTransactionRepository walletTransactionRepository,
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            IPaymentRepository paymentRepository,
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator,
            IPaymentGatewayService paymentGatewayService,
            ILogger<VerifyPaymentCommandHandler> logger)
        {
            this.walletTransactionRepository = walletTransactionRepository;
            this.paymentRepository = paymentRepository;
            this.walletRepository = walletRepository;
            this.unitOfWork = unitOfWork;
            this.paymentGatewayService = paymentGatewayService;
            _httpContextAccessor = httpContextAccessor;
            this.logger = logger;
            this.mediator = mediator;
            this.auditLogRepository = auditLogRepository;
            this.userRepository = userRepository;
        }

        public async Task<Result<PaystackVerifyResponse>> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await paymentRepository.GetByReferenceAsync(request.Reference);
                if (payment == null)
                {
                    logger.LogWarning("Payment not found for reference {Reference}", request.Reference);
                    return Result<PaystackVerifyResponse>.Failure("Payment not found");
                }

                if (payment.Status == PaymentStatus.Completed)
                {
                    logger.LogInformation("Payment {Reference} already verified", request.Reference);
                    return Result<PaystackVerifyResponse>.Failure("Payment Already Verified");
                }
                var deliveryInfo = _httpContextAccessor.HttpContext?.Session.GetDeliveryInfo();
                if (payment.Purpose == PaymentPurpose.OrderPayment && deliveryInfo == null)
                {
                    logger.LogWarning("Delivery information is missing in session for Customer {CustomerId}", payment.PayerId);
                    return Result<PaystackVerifyResponse>.Failure("Delivery information is required for wallet payments");
                }
                var user = await userRepository.GetByEmailAsync((string)payment.Payer.Email);
                if (user == null)
                {
                    logger.LogWarning("Customer with ID {CustomerId} not found", payment.PayerId);
                    return Result<PaystackVerifyResponse>.Failure("User not found");
                }

                var verifyResponse = await paymentGatewayService.VerifyTransactionAsync(request.Reference);

                if (verifyResponse.Data.Status.Equals("success",StringComparison.OrdinalIgnoreCase))
                {
                    payment.MarkAsCompleted();
                    if (payment.Purpose == PaymentPurpose.WalletFunding)
                    {
                        var wallet = await walletRepository.GetByUserIdAsync(payment.PayerId);
                        if (wallet == null)
                            return Result<PaystackVerifyResponse>.Failure("Wallet not found");

                        wallet.Credit(payment.Amount);
                        var transaction = new WalletTransaction(wallet.Id, payment.Amount, TransactionType.Credit, payment.PaymentReference,payment.Id, payment.Note);
                        await walletTransactionRepository.AddAsync(transaction);
                        //payment.LinkToTransaction(transaction.Id);
                    }

                    await unitOfWork.SaveChangesAsync(cancellationToken);
                    await auditLogRepository.AddAsync(new AuditLog(
                       userId: user.Id,
                       action: "Payment Successful",
                       entityName: "Payment",
                       entityId: payment.Id,
                       details: $"Payment of {payment.Amount:C} was successful. Reference: {payment.PaymentReference}",
                       ip: request.RequestMetadata.IpAddress,
                       userAgent: request.RequestMetadata.UserAgent
                   ));
                    await mediator.Publish(new PaymentStatusChangedEvent(payment.Id,payment.PayerId,payment.Status,payment.Amount,payment.PaymentReference));
                    logger.LogInformation("Payment {Reference} verified successfully", request.Reference);

                    if (payment.Purpose == PaymentPurpose.OrderPayment)
                    {
                        var dto = new CreateSalesOrderRequestModel(deliveryInfo!.AddressId!.Value, deliveryInfo.ETA ?? DateTime.Now, deliveryInfo.Cost!.Value, payment.PaymentReference);
                        var result = await mediator.Send(new CreateSalesOrderCommand(dto, request.RequestMetadata));
                    }
                   
                    return Result<PaystackVerifyResponse>.Success(verifyResponse);
                }

                payment.MarkAsFailed();
                await unitOfWork.SaveChangesAsync(cancellationToken);
                await auditLogRepository.AddAsync(new AuditLog(
           userId: user.Id,
           action: "Payment Failed",
           entityName: "Payment",
           entityId: payment.Id,
           details: $"Payment of {payment.Amount:C} failed. Reference: {payment.PaymentReference}",
           ip: request.RequestMetadata.IpAddress,
           userAgent: request.RequestMetadata.UserAgent
       ));
                await mediator.Publish(new PaymentStatusChangedEvent(payment.Id, payment.PayerId, payment.Status, payment.Amount, payment.PaymentReference));
                return Result<PaystackVerifyResponse>.Failure("Payment verification failed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error verifying payment with reference {Reference}", request.Reference);
                return Result<PaystackVerifyResponse>.Failure("Error verifying payment");
            }
        }
    }

}
