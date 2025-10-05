using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.SalesOrders.Create
{
    public class CreateSalesOrderCommandHandler : IRequestHandler<CreateSalesOrderCommand, Result<Guid>>
    {
        private readonly IDeliveryAddressRepository _deliveryAddressRepository;
        private readonly IDeliveryAssignmentRepository _deliveryAssignmentRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMediator _mediator;
        private readonly IFezService _fezService;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ISalesOrderItemRepository _salesOrderItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceItemRepository _invoiceItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateSalesOrderCommandHandler> _logger;
        private readonly IPaymentRepository _paymentRepository;

        public CreateSalesOrderCommandHandler(
            IDeliveryAddressRepository deliveryAddressRepository,
            IPublishEndpoint publishEndpoint,
            IMediator mediator,
            IDeliveryAssignmentRepository deliveryAssignmentRepository,
            IFezService fezService,
            ISalesOrderRepository salesOrderRepository,
            ISalesOrderItemRepository salesOrderItemRepository,
            ICartRepository cartRepository,
            IInvoiceRepository invoiceRepository,
            IInvoiceItemRepository invoiceItemRepository,
            IPaymentRepository paymentRepository,
            ICustomerRepository customerRepository,
            IAuditLogRepository auditLogRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateSalesOrderCommandHandler> logger)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _deliveryAddressRepository = deliveryAddressRepository;
            _deliveryAssignmentRepository = deliveryAssignmentRepository;
            _fezService = fezService;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderItemRepository = salesOrderItemRepository;
            _cartRepository = cartRepository;
            _invoiceRepository = invoiceRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _customerRepository = customerRepository;
            _auditLogRepository = auditLogRepository;
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Guid>> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
        {
            var deliveryAddress = await _deliveryAddressRepository.GetByIdAsync(request.CreateSalesOrderRequestModel.DeliveryAddressId);
            if (deliveryAddress == null)
                return Result<Guid>.Failure("Delivery address not found");

            var customer = await _customerRepository.GetByIdAsync(deliveryAddress.CustomerId);
            if (customer == null)
                return Result<Guid>.Failure("Customer not found");

            var user = await _userRepository.GetByEmailAsync((string)customer.Email);
            if (user == null)
                return Result<Guid>.Failure("User not found");

            try
            {
                var cart = await _cartRepository.GetByUserIdAsync(user.Id);
                if (cart == null || !cart.Items.Any())
                    return Result<Guid>.Failure("Cart is empty");

                await _unitOfWork.BeginTransactionAsync();

               
                var order = new SalesOrder(
                    orderNumber: $"SO-{Guid.NewGuid():N}",
                    customerId: customer.Id,
                    discount: 0,
                    tax: 0,
                    expectedDeliveryDate: request.CreateSalesOrderRequestModel.ExpectedDeliveyDate,
                    note: $"Sales Order For {(string)customer.Email}"
                );

                await _salesOrderRepository.AddAsync(order);

                 
                var invoice = new Invoice(
                    invoiceNumber: $"INV-{Guid.NewGuid():N}",
                    customerId: customer.Id,
                    discount: order.Discount,
                    tax: order.Tax
                );

                invoice.AddSalesOrder(order.Id);

                await _invoiceRepository.AddAsync(invoice);

                order.AddInvoice(invoice.Id);

                var fezRequestItems = new List<CreateFezOrderRequestItem>();
                int itemIndex = 1;

                foreach (var cartItem in cart.Items)
                {
                    var orderItem = new SalesOrderItem(
                        salesOrderId: order.Id,
                        productId: cartItem.ProductId,
                        productName: cartItem.Product.Name,
                        quantity: cartItem.Quantity,
                        unitPrice: cartItem.Product.SellingPrice
                    );

                    var uniqueId = $"{order.OrderNumber}-{itemIndex++}";

                    var fezRequest = new CreateFezOrderRequestItem
                    {
                        RecipientAddress = deliveryAddress.Street,
                        RecipientState = deliveryAddress.State.Name,
                        RecipientName = deliveryAddress.FullName,
                        RecipientPhone = deliveryAddress.PhoneNumber,
                        RecipientEmail = deliveryAddress.Email,
                        UniqueID = uniqueId,
                        BatchID = order.OrderNumber,
                        ValueOfItem = cartItem.Product.SellingPrice * cartItem.Quantity,
                        Weight = (decimal)cartItem.Product.Weight,  
                        ItemDescription = cartItem.Product.Name
                    };

                    fezRequestItems.Add(fezRequest);

                    await _salesOrderItemRepository.AddAsync(orderItem);

                    var invoiceItem = new InvoiceItem(
                        productId: cartItem.ProductId,
                        description: cartItem.Product.Name,
                        quantity: cartItem.Quantity,
                        unitPrice: cartItem.Product.SellingPrice,
                        invoiceId: invoice.Id
                    );
                    await _invoiceItemRepository.AddAsync(invoiceItem);
                }

               
                var fezResponse = await _fezService.CreateOrderAsync(fezRequestItems);

                if (!fezResponse.Success || fezResponse.Data is null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result<Guid>.Failure($"Fez order failed: {fezResponse.Message}");
                }

                

                foreach (var (uniqueId, fezOrderNo) in fezResponse.Data.OrderNos)
                {
                    var item = order.Items.FirstOrDefault(i => i.UniqueId == uniqueId);
                    if (item != null)
                        item.SetTracking(uniqueId, fezOrderNo);
                }

                var batchId = fezResponse.Data.OrderNos.Values.First();

                var deliveryAssignment = new DeliveryAssignment(
                   salesOrderId: order.Id,
                   deliveryAddressId: deliveryAddress.Id,
                   deliveryFee: request.CreateSalesOrderRequestModel.DeliveryCost,
                   externalJobId: batchId,
                   email: deliveryAddress.Email,
                   phone: deliveryAddress.PhoneNumber,
                   name: deliveryAddress.FullName,
                   externalService: "Fez",
                   note: $"Delivey Assignment Created For Order With Order Number {order.OrderNumber}"
               );

                await _deliveryAssignmentRepository.AddAsync(deliveryAssignment);

                order.AddDeliveryAssignment(deliveryAssignment.Id);

                order.RecalculateTotals();
                invoice.RecalculateSubTotal();
                invoice.MarkAsPaid();

                var payment = await _paymentRepository.GetByReferenceAsync(request.CreateSalesOrderRequestModel.paymentReference);
                if (payment == null)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result<Guid>.Failure("Payment not found");
                }

                payment.AddInvoice(invoice.Id);

                cart.ClearItems();

                await _unitOfWork.CommitTransactionAsync();

                await _auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "CreateOrder",
                nameof(SalesOrder),
                order.Id,
                $"Order {order.OrderNumber} created. Delivery assigned with ExternalJobId {deliveryAssignment.ExternalJobId}.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
                ));

                var productDtos = cart.Items.Select(i => new OrderCreatedProductDto
                {
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Sku = i.Product.SKU,
                    ImageUrl = i.Product.ImageUrl,   
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.SellingPrice,
                }).ToList();

                var orderCreatedEvent = new OrderCreatedEvent(
                    order.Id,
                    userId: user.Id,
                    order.OrderNumber,
                    (string)customer.Email,
                    customer.FullName,
                    deliveryAssignment.ExternalJobId ?? "",   
                    invoice.SubTotal,
                    deliveryAssignment.DeliveryFee,
                    invoice.SubTotal + deliveryAssignment.DeliveryFee,
                    productDtos
                );

                await _mediator.Publish(orderCreatedEvent, cancellationToken);
                await _publishEndpoint.Publish(orderCreatedEvent, cancellationToken);


                return Result<Guid>.Success(order.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create sales order");

                await _auditLogRepository.AddAsync(new AuditLog(
                    user.Id,
                    "CreateOrderFailed",
                    nameof(SalesOrder),
                    null,
                    $"Order creation failed: {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));
                return Result<Guid>.Failure("An unexpected error occurred while creating the order.");
            }
        }
    }
}
