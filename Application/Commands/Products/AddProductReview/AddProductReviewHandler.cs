using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Products.AddProductReview
{
    public class AddProductReviewCommandHandler : IRequestHandler<AddProductReviewCommand, Result<Guid>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<AddProductReviewCommandHandler> _logger;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AddProductReviewCommandHandler(
            IReviewRepository reviewRepository,
            ILogger<AddProductReviewCommandHandler> logger,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IAuditLogRepository auditLogRepository)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<Result<Guid>> Handle(AddProductReviewCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var review = new Review(
                reviewerId: _authService.CurrentUser()!.Id,
                rating: dto.Rating,
                comment: dto.Comment ?? string.Empty,
                productId: dto.ProductId
            );
            await _unitOfWork.BeginTransactionAsync();

            await _reviewRepository.AddAsync(review);

            await _auditLogRepository.AddAsync(new AuditLog(
                userId: _authService.CurrentUser()!.Id,
                action: "Reviewed Product",
                entityName: "Review",
                entityId: review.Id,
                details: $"Rating: {dto.Rating}, Comment: {dto.Comment ?? "[No comment]"}",
                ip: request.RequestMetadata.IpAddress,
                userAgent: request.RequestMetadata.UserAgent
            ));

            await _unitOfWork.CommitTransactionAsync();


            _logger.LogInformation("Review created by {UserId} for Product {ProductId}", _authService.CurrentUser()!.Id, dto.ProductId);

            return Result<Guid>.Success(review.Id);
        }
    }

}
