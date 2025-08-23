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

namespace Application.Commands.Tags.Delete
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Result<string>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<DeleteTagCommandHandler> _logger;
        private readonly IMemoryCacheService _memoryCacheService;

        public DeleteTagCommandHandler(
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IAuditLogRepository auditLogRepository,
            ILogger<DeleteTagCommandHandler> logger,
            IMemoryCacheService memoryCacheService)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Result<string>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to delete tag with ID {TagId}", request.Id);

            var tag = await _tagRepository.GetByIdAsync(request.Id);
            if (tag == null)
            {
                _logger.LogWarning("Delete failed. Tag with ID {TagId} not found.", request.Id);
                return Result<string>.Failure("Tag not found");
            }

            var userId = _authService.CurrentUser()!.Id;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _tagRepository.DeleteAsync(tag);

                var auditLog = new AuditLog(
                    userId,
                    "Delete",
                    nameof(Tag),
                    tag.Id,
                    $"Deleted tag with name '{tag.Name}'",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );

                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.CommitTransactionAsync();
                await _memoryCacheService.RemoveCacheByPrefix("GetAllTagsPaginated_", "GetTagsByProductId-", "Tag_");
                _logger.LogInformation("Tag with ID {TagId} deleted successfully.", tag.Id);
                return Result<string>.Success($"Tag '{tag.Name}' deleted successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while deleting tag with ID {TagId}", tag.Id);
                throw;
            }
        }
    }

}
