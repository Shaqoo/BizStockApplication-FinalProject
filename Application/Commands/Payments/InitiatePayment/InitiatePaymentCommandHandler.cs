using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
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

        public InitiatePaymentCommandHandler(
            IPaymentGatewayService paymentGatewayService,
            IMediator mediator,
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            IPaymentRepository paymentRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork,
            ILogger<InitiatePaymentCommandHandler> logger)
        {
            _mediator = mediator;
            _userRepository = userRepository;
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

            var reference = $"PAY-{Guid.NewGuid():N}";

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
