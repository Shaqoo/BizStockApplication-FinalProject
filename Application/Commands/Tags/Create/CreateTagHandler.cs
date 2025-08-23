using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.Tags.Create
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result<Guid>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<CreateTagCommandHandler> _logger;
        private readonly IMemoryCacheService _memoryCacheService;

        public CreateTagCommandHandler(
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IAuditLogRepository auditLogRepository,
            ILogger<CreateTagCommandHandler> logger,
            IMemoryCacheService memoryCacheService)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to create a new tag with name: {TagName}", request.Request.Name);

            var exist = await _tagRepository.ExistsByNameAsync(request.Request.Name);
            if (exist)
            {
                _logger.LogWarning("Tag creation failed. Name '{TagName}' already exists.", request.Request.Name);
                return Result<Guid>.Failure("Name Already Exists");
            }

            var tag = new Tag(request.Request.Name);
            var userId = _authService.CurrentUser()!.Id;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _tagRepository.AddAsync(tag);

                var auditLog = new AuditLog(
                    userId: userId,
                    action: "Create",
                    entityName: nameof(Tag),
                    entityId: tag.Id,
                    details: $"Created new tag with Name = '{tag.Name}'",
                    ip:request.RequestMetadata.IpAddress,
                    userAgent: request.RequestMetadata.UserAgent
                );

                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.CommitTransactionAsync();
                await _memoryCacheService.RemoveCacheByPrefix("GetAllTagsPaginated_", "GetTagsByProductId-", "Tag_");

                _logger.LogInformation("Tag '{TagName}' created successfully with ID {TagId}", tag.Name, tag.Id);

                return Result<Guid>.Success(tag.Id);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while creating tag '{TagName}'", request.Request.Name);
                throw;
            }
        }
    }

}
