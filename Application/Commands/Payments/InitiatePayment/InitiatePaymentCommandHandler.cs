using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Payments.InitiatePayment
{
    public class InitiatePaymentCommandHandler
    : IRequestHandler<InitiatePaymentCommand, Result<string>>
    {
        private readonly IPaymentGatewayService _paymentGatewayService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<InitiatePaymentCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionRepository _walletTransactionRepository;
        public InitiatePaymentCommandHandler(
            IPaymentGatewayService paymentGatewayService,
            IWalletTransactionRepository walletTransactionRepository,
            IMediator mediator,
            IWalletRepository walletRepository,
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            IPaymentRepository paymentRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            ILogger<InitiatePaymentCommandHandler> logger)
        {
            _walletTransactionRepository = walletTransactionRepository;
            _mediator = mediator;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _auditLogRepository = auditLogRepository;
            _paymentGatewayService = paymentGatewayService;
            _paymentRepository = paymentRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(
            InitiatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            var request = command.Request;

            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found", request.CustomerId);
                return Result<string>.Failure("Customer not found");
            }
            var user = await _userRepository.GetByEmailAsync((string)customer.Email);
            if (user == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found", request.CustomerId);
                return Result<string>.Failure("Customer not found");
            }

            if(request.PaymentPurpose == PaymentPurpose.WalletFunding)
            {
                var wallet = await _walletRepository.GetByUserIdAsync(customer.Id);
                if (wallet == null)
                {
                    _logger.LogWarning("Customer with ID {CustomerId} Wallet not found", request.CustomerId);
                    return Result<string>.Failure("Wallet not found");
                }
            }

            

            var reference = $"PAY-{Guid.NewGuid():N}";


            if (request.Method == PaymentMethod.Wallet)
            {
                try
                {
                    var wallet = await _walletRepository.GetByUserIdAsync(customer.Id);
                    if (wallet == null)
                    {
                        _logger.LogWarning("Customer with ID {CustomerId} Wallet not found", request.CustomerId);
                        return Result<string>.Failure("Wallet not found");
                    }

                    bool isValidPin = BCrypt.Net.BCrypt.Verify(request.Pin.ToString(), wallet.PinHash);
                    if (!isValidPin)
                    {
                        _logger.LogWarning("Invalid Pin for Customer {CustomerId}", request.CustomerId);
                        return Result<string>.Failure("Invalid Pin");
                    }

                    var hasSufficientBalance = await _walletRepository.HasSufficientBalanceAsync(customer.Id, request.Amount);
                    if (!hasSufficientBalance)
                    {
                        _logger.LogWarning("Insufficient wallet balance for Customer {CustomerId}", request.CustomerId);
                        return Result<string>.Failure("Insufficient wallet balance");
                    }

                    
                    var payment = new Payment(
                        reference,
                        customer.Id,
                        request.Amount,
                        request.Method,
                        request.PaymentPurpose,
                        null, null,
                        $"Payment for {request.PaymentPurpose}"
                    );
 
                    var walletTransaction = new WalletTransaction(
                        customer.Id,
                        request.Amount,
                        TransactionType.Debit,
                        reference,
                        payment.Id,
                        payment.Note
                    );

                 
                    wallet.Debit(request.Amount);

                   
                    await _paymentRepository.AddAsync(payment);
                    await _walletTransactionRepository.AddAsync(walletTransaction);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                   
                    payment.MarkAsCompleted();
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                 
                    await _auditLogRepository.AddAsync(new AuditLog(
                        userId: user.Id,
                        action: "Payment Completed",
                        entityName: "Payment",
                        entityId: payment.Id,
                        details: $"Payment of {payment.Amount:C} completed. Reference: {payment.PaymentReference}",
                        ip: command.RequestMetadata.IpAddress,
                        userAgent: command.RequestMetadata.UserAgent
                    ));

                    return Result<string>.Success("Wallet payment completed successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing wallet payment for Customer {CustomerId}", request.CustomerId);
                    return Result<string>.Failure("An unexpected error occurred while processing wallet payment. Please try again.");
                }
            }

            try
            {
                var paymentLink = await _paymentGatewayService
                    .InitializeTransactionAsync(request.Amount, (string)customer.Email, reference);


                var payment = new Payment(reference, customer.Id, request.Amount, request.Method, request.PaymentPurpose
                    , request.WalletTransactionId, request.InvoiceId, request.Note);

                await _paymentRepository.AddAsync(payment);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _auditLogRepository.AddAsync(new AuditLog(
                userId: user.Id,
                action: "Payment Initialized",
                entityName: "Payment",
                entityId: payment.Id,
                details: $"Payment of {payment.Amount:C} initialized. Reference: {payment.PaymentReference}",
                ip: command.RequestMetadata.IpAddress,
                userAgent: command.RequestMetadata.UserAgent
            ));


                await _mediator.Publish(new PaymentInitializedEvent(
                    payment.Id,
                    payment.PayerId,
                    payment.Amount,
                    payment.PaymentReference
                ));


                _logger.LogInformation("Payment initialized for Customer {CustomerId} with Reference {Reference}",
                    request.CustomerId, reference);

                return Result<string>.Success(paymentLink);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing payment for Customer {CustomerId}", request.CustomerId);
                return Result<string>.Failure("Failed to initialize payment");
            }
        }
    }

}
