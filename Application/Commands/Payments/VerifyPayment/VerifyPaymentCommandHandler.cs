using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Application.Commands.Payments.VerifyPayment
{
    public class VerifyPaymentCommandHandler : IRequestHandler<VerifyPaymentCommand, Result<bool>>
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
        public VerifyPaymentCommandHandler(
            IWalletTransactionRepository walletTransactionRepository,
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            IPaymentRepository paymentRepository,
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator,
            IPaymentGatewayService paymentGatewayService,
            ILogger<VerifyPaymentCommandHandler> logger)
        {
            this.walletTransactionRepository = walletTransactionRepository;
            this.paymentRepository = paymentRepository;
            this.walletRepository = walletRepository;
            this.unitOfWork = unitOfWork;
            this.paymentGatewayService = paymentGatewayService;
            this.logger = logger;
            this.mediator = mediator;
            this.auditLogRepository = auditLogRepository;
            this.userRepository = userRepository;
        }

        public async Task<Result<bool>> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await paymentRepository.GetByReferenceAsync(request.Reference);
                if (payment == null)
                {
                    logger.LogWarning("Payment not found for reference {Reference}", request.Reference);
                    return Result<bool>.Failure("Payment not found");
                }

                if (payment.Status == PaymentStatus.Completed)
                {
                    logger.LogInformation("Payment {Reference} already verified", request.Reference);
                    return Result<bool>.Success(true);
                }
                var user = await userRepository.GetByEmailAsync((string)payment.Payer.Email);
                if (user == null)
                {
                    logger.LogWarning("Customer with ID {CustomerId} not found", payment.PayerId);
                    return Result<bool>.Failure("User not found");
                }

                var status = await paymentGatewayService.VerifyTransactionAsync(request.Reference);

                if (status == "success")
                {
                    payment.MarkAsCompleted();
                    if (payment.Purpose == PaymentPurpose.WalletFunding)
                    {
                        var wallet = await walletRepository.GetByUserIdAsync(payment.PayerId);
                        if (wallet == null)
                            return Result<bool>.Failure("Wallet not found");

                        wallet.Credit(payment.Amount);
                        var transaction = new WalletTransaction(wallet.Id, payment.Amount, TransactionType.Credit, payment.PaymentReference, payment.Note, payment.Id);
                        await walletTransactionRepository.AddAsync(transaction);
                        payment.LinkToTransaction(transaction.Id);
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
                    return Result<bool>.Success(true);
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
                return Result<bool>.Failure("Payment verification failed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error verifying payment with reference {Reference}", request.Reference);
                return Result<bool>.Failure("Error verifying payment");
            }
        }
    }

}
