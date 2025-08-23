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

namespace Application.Commands.Tags.Update
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Result<string>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ILogger<UpdateTagCommandHandler> _logger;
        private readonly IMemoryCacheService memoryCacheService;

        public UpdateTagCommandHandler(
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            IAuditLogRepository auditLogRepository,
            ILogger<UpdateTagCommandHandler> logger,
            IMemoryCacheService memoryCacheService)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _auditLogRepository = auditLogRepository;
            _logger = logger;
            this.memoryCacheService = memoryCacheService;
        }

        public async Task<Result<string>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update tag with ID {TagId}", request.Request.Id);

            var tag = await _tagRepository.GetByIdAsync(request.Request.Id);
            if (tag == null)
            {
                _logger.LogWarning("Update failed. Tag with ID {TagId} not found.", request.Request.Id);
                return Result<string>.Failure("Tag not found");
            }

            var exists = await _tagRepository.ExistsByNameAsync(request.Request.Name);
            if (exists)
            {
                _logger.LogWarning("Update failed. Another tag with name '{TagName}' already exists.", request.Request.Name);
                return Result<string>.Failure("Name already exists");
            }

            var oldName = tag.Name;
            tag.UpdateName(request.Request.Name);

            var userId = _authService.CurrentUser()!.Id;

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _tagRepository.UpdateAsync(tag);

                var auditLog = new AuditLog(
                    userId,
                    "Update",
                    nameof(Tag),
                    tag.Id,
                    $"Updated tag name from '{oldName}' to '{tag.Name}'",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                );

                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.CommitTransactionAsync();
                await memoryCacheService.RemoveCacheByPrefix("GetAllTagsPaginated_", "GetTagsByProductId-", "Tag_");
                _logger.LogInformation("Tag with ID {TagId} updated successfully.", tag.Id);
                return Result<string>.Success("Tag Updated Successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Error occurred while updating tag with ID {TagId}", tag.Id);
                throw;
            }
        }
    }

}
