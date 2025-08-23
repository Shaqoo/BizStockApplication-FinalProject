using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using Domain.Enums;
using Mailjet.Client.Resources;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.UpdateLostAccessRequest
{
    public class UpdateLostAccessRequestCommandHandler
        : IRequestHandler<UpdateLostAccessRequestCommand, Result<Guid>>
    {
        private readonly ILostAccessRequestRepository _lostAccessRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<UpdateLostAccessRequestCommandHandler> _logger;
        private readonly IMediator _mediator;

        public UpdateLostAccessRequestCommandHandler(
            ILostAccessRequestRepository lostAccessRepository,
            IUserRepository userRepository,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IMediator mediator,
            ILogger<UpdateLostAccessRequestCommandHandler> logger)
        {
            _lostAccessRepository = lostAccessRepository;
            _userRepository = userRepository;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _mediator = mediator;
        }

        public async Task<Result<Guid>> Handle(UpdateLostAccessRequestCommand request, CancellationToken cancellationToken)
        {
            var lostAccess = await _lostAccessRepository.GetByIdAsync(request.RequestId, cancellationToken);
            if (lostAccess == null)
                return Result<Guid>.Failure("Lost access request not found.");

            var currentUser = _authService.CurrentUser();
            if (currentUser == null)
                return Result<Guid>.Failure("Unauthorized access.");

            var user = await _userRepository.GetByEmailAsync(lostAccess.UserIdentifier);
            if (user == null)
                return Result<Guid>.Failure("Associated user not found.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (request.Dto.Status == LostAccessStatus.Resolved)
                {
                   
                    user.DisableTwoFactor();
                    lostAccess.Approve(request.Dto.AdminNotes);

                    await _lostAccessRepository.UpdateAsync(lostAccess, cancellationToken);
                    await _userRepository.UpdateUserAsync(user);

                    _logger.LogInformation(
                        "LostAccessRequest {RequestId} resolved. Disabled MFA for User {UserId} by Admin {AdminId}.",
                        lostAccess.Id, user.Id, currentUser.Id);

                    var auditLog = new AuditLog(
                        currentUser.Id,
                        action: "UpdateLostAccessRequest-Resolved",
                        entityName: nameof(lostAccess),
                        entityId: lostAccess.Id,
                        details: $"MFA disabled for User {user.Id}, Request marked Resolved with notes: {request.Dto.AdminNotes}"
                    );

                    await _mediator.Publish(
                        new LostAccessRequestApprovedEvent(lostAccess.Id,(string)user.Email,user.FullName,lostAccess.AdminNotes ?? string.Empty,
                        lostAccess.Status.ToString(),lostAccess.SubmittedAt),
                        cancellationToken);
                    await _auditLogRepository.AddAsync(auditLog);
                }
                else
                {
                    lostAccess.Reject(request.Dto.AdminNotes);
                    await _lostAccessRepository.UpdateAsync(lostAccess, cancellationToken);

                    _logger.LogInformation(
                        "LostAccessRequest {RequestId} updated to {Status} by Admin {AdminId}.",
                        lostAccess.Id, request.Dto.Status, currentUser.Id);

                    await _mediator.Publish(
                       new LostAccessRequestRejectedEvent(lostAccess.Id, (string)user.Email, user.FullName, lostAccess.AdminNotes ?? string.Empty,
                       lostAccess.Status.ToString(), lostAccess.SubmittedAt),
                       cancellationToken);

                    var auditLog = new AuditLog(
                        currentUser.Id,
                        action: "UpdateLostAccessRequest-Rejected",
                        entityName: nameof(lostAccess),
                        entityId: lostAccess.Id,
                        details: $"Request rejected with notes: {request.Dto.AdminNotes}"
                    );

                    await _auditLogRepository.AddAsync(auditLog);
                }

                await _unitOfWork.CommitTransactionAsync();
                return Result<Guid>.Success(lostAccess.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error updating LostAccessRequest {RequestId}", request.RequestId);
                return Result<Guid>.Failure("Failed to update lost access request.");
            }
        }
    }
}
