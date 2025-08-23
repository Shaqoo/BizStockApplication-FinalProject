using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Users.LossAccountRequest
{
    public class CreateLostAccessRequestHandler
        : IRequestHandler<CreateLostAccessRequestCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILostAccessRequestRepository _lostAccessRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateLostAccessRequestHandler> _logger;

        public CreateLostAccessRequestHandler(
            IUserRepository userRepository,
            ILostAccessRequestRepository lostAccessRepository,
            IUnitOfWork unitOfWork,
            IAuditLogRepository auditLogRepository,
            ILogger<CreateLostAccessRequestHandler> logger)
        {
            _userRepository = userRepository;
            _lostAccessRepository = lostAccessRepository;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateLostAccessRequestCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

           
            var user = await _userRepository.GetByEmailAsync(dto.UserIdentifier);
            if (user == null)
            {
                _logger.LogWarning("Lost access request failed. Email {Email} not found.", dto.UserIdentifier);
                return Result<Guid>.Failure("No account found with this email.");
            }

            
            var existingRequest = await _lostAccessRepository.SearchByEmailAsync(dto.UserIdentifier);
            if (existingRequest != null && existingRequest.Status == LostAccessStatus.Pending)
            {
                _logger.LogInformation("Blocked lost access request for {Email} because one is already pending.", dto.UserIdentifier);
                return Result<Guid>.Failure("You already have a pending lost access request.");
            }

            var lostAccessRequest = new LostAccessRequest(
                dto.UserIdentifier,
                dto.ProblemDescription,
                dto.AlternateEmail ?? string.Empty,
                dto.AlternatePhone
            );

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _lostAccessRepository.AddAsync(lostAccessRequest);

                var auditLog = new AuditLog(
                    user.Id,
                    "CreateLostAccessRequest",
                    nameof(LostAccessRequest),
                    lostAccessRequest.Id,
                    $"User {user.Email} submitted lost access request",
                    request.RequestMetadata.IpAddress,  
                    request.RequestMetadata.UserAgent   
                );

                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("Lost access request created for {Email}", dto.UserIdentifier);

                return Result<Guid>.Success(lostAccessRequest.Id, "Lost access request submitted successfully.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Failed to create lost access request for {Email}", dto.UserIdentifier);
                return Result<Guid>.Failure("An error occurred while processing your request.");
            }
        }
    }
}
