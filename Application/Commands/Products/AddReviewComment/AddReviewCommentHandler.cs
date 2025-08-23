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

namespace Application.Commands.Products.AddReviewComment
{
    public class AddReviewCommentCommandHandler : IRequestHandler<AddReviewCommentCommand, Result<string>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<AddReviewCommentCommandHandler> _logger;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        public AddReviewCommentCommandHandler(
            IReviewRepository reviewRepository,
            ILogger<AddReviewCommentCommandHandler> logger,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IAuditLogRepository auditLogRepository)
        {
            _reviewRepository = reviewRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _auditLogRepository = auditLogRepository;
            _authService = authService;
        }

        public async Task<Result<string>> Handle(AddReviewCommentCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.ReviewId);
            if (review is null || review.ReviewerId != _authService.CurrentUser()!.Id)
                return Result<string>.Failure("Review not found or access denied.");

            review.AddComment(request.Comment);  

            await _reviewRepository.UpdateAsync(review);

            await _unitOfWork.BeginTransactionAsync();

            await _auditLogRepository.AddAsync(new AuditLog(
               userId: _authService.CurrentUser()!.Id,
               action: "Added Comment to Review",
               entityName: "Review",
               entityId: review.Id,
               details: $"Comment: '{request.Comment}'",
               ip: request.RequestMetadata.IpAddress,
               userAgent: request.RequestMetadata.UserAgent
           ));

            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation("User {UserId} added comment to review {ReviewId}",_authService.CurrentUser()!.Id, request.ReviewId);

            return Result<string>.Success("");
        }
    }

}
