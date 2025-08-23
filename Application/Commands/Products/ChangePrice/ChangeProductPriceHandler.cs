using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Products.ChangePrice
{
    public class ChangeProductPriceHandler(
    IAuthService authService,
    IUnitOfWork unitOfWork,
    IProductRepository productRepository,
    IAuditLogRepository logRepository,
    IMediator mediator,
    ILogger<ChangeProductPriceHandler> logger)
    : IRequestHandler<ChangeProductPriceCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(ChangeProductPriceCommand request, CancellationToken cancellationToken)
        {
            var user = authService.CurrentUser();
            if (user == null)
                return Result<string>.Failure("User not found.");

            if (user.RoleName is not ("Admin" or "Manager" or "Inventory Manager"))
                return Result<string>.Failure("You are not authorized to change product prices.");

            var product = await productRepository.GetByIdAsync(request.Change.ProductId);
            if (product == null)
                return Result<string>.Failure("Product not found.");

            var oldCostPrice = product.CostPrice;
            var oldSellingPrice = product.SellingPrice;

            if (oldCostPrice == request.Change.CostPrice && oldSellingPrice == request.Change.SellingPrice)
                return Result<string>.Failure("No change detected in cost or selling price.");

            await unitOfWork.BeginTransactionAsync();

            product.UpdatePrices(request.Change.CostPrice, request.Change.SellingPrice);
            await productRepository.UpdateAsync(product);

            await logRepository.AddAsync(new AuditLog(
                user.Id,
                "ChangeProductPrice",
                nameof(Product),
                product.Id,
                $"Old CP: {oldCostPrice}, Old SP: {oldSellingPrice} → New CP: {request.Change.CostPrice}, New SP: {request.Change.SellingPrice}",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent));

            await unitOfWork.CommitTransactionAsync();

            logger.LogInformation("Product {ProductId} price updated: CP {OldCP}->{NewCP}, SP {OldSP}->{NewSP} by {Email}",
                product.Id, oldCostPrice, request.Change.CostPrice, oldSellingPrice, request.Change.SellingPrice, user.Email);
            
            await mediator.Publish(new ProductPriceChangedEvent(
                product.Id,product.Name,oldSellingPrice, request.Change.SellingPrice,user.Id), cancellationToken);

            return Result<string>.Success("Product prices updated successfully.");
        }
    }

}
