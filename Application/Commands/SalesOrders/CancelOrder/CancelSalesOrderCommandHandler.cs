using Application.Commands.Refunds.ProcessRefund;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.SalesOrders.CancelOrder
{
    public class CancelSalesOrderCommandHandler(ISalesOrderRepository salesOrderRepository,
        IFezService fezService,
        ILogger<CancelSalesOrderCommandHandler> logger,
        IInvoiceRepository invoiceRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator) : IRequestHandler<CancelSalesOrderCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CancelSalesOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            if (order == null)
            {
                logger.LogWarning("Order with ID {SalesOrderId} not found", request.SalesOrderId);
                return Result<string>.Failure("Order not found");
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                logger.LogInformation("Order {SalesOrderId} is already cancelled", request.SalesOrderId);
                return Result<string>.Failure("Order is already cancelled");
            }
            if (order.Status == OrderStatus.Completed)
            {
                logger.LogWarning("Order {SalesOrderId} cannot be cancelled as it is already {Status}", request.SalesOrderId, order.Status);
                return Result<string>.Failure($"Order cannot be cancelled as it is already {order.Status}");
            }
            var invoice = await invoiceRepository.GetByExpression(a => a.SalesOrderId == order.Id);
            if (invoice == null)
            {
                logger.LogWarning("Invoice not found for Order {SalesOrderId}", request.SalesOrderId);
                return Result<string>.Failure("Invoice not found for the order");
            }
            if (invoice != null && invoice.Status == InvoiceStatus.Cancelled)
            {
                logger.LogWarning("Order {SalesOrderId} cannot be cancelled as its invoice is already {status}", request.SalesOrderId,InvoiceStatus.Cancelled);
                return Result<string>.Failure("Order cannot be cancelled as its invoice is already paid");
            }

            var payment = invoice!.Payments.FirstOrDefault();
            if(payment == null)
            {
                logger.LogWarning("No payment found for Invoice {InvoiceId}", invoice.Id);
                return Result<string>.Failure("No payment found for the invoice");
            }

            invoice.MarkAsCancelled();

            order.MarkAsCancelled();

            await unitOfWork.CommitTransactionAsync();

            foreach (var item in order.Items)
            {
                await fezService.CancelOrderAsync(item.UniqueId!);
            }
            logger.LogInformation("Order {SalesOrderId} marked as cancelled", request.SalesOrderId);

            var response = await mediator.Send(new ProcessRefundCommand(order.Id,payment.Amount,payment.Method,"",payment.PaymentReference,request.RequestMetadata));
            if (response.IsSuccess)
            {
                logger.LogInformation("Order {SalesOrderId} has been cancelled successfully", request.SalesOrderId);
                logger.LogInformation("Refund of {Amount:N2} processed successfully for Order {SalesOrderId}", payment.Amount, request.SalesOrderId);
                return Result<string>.Success("Order cancelled and refund processed successfully");
            }
            else
            {
                logger.LogWarning("Refund processing failed for Order {SalesOrderId}: {Error}", request.SalesOrderId, response.Message);
                return Result<string>.Failure($"Order cancelled but refund processing failed: {response.Message}");
            }
        }
    }
}
