using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fido2NetLib.AuthenticatorAttestationRawResponse;

namespace Application.Commands.Brands.Update
{
    public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand, Result<Guid>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogRepository _auditLogService;
        private readonly IAuthService _authService;

        public UpdateBrandCommandHandler(
            IBrandRepository brandRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork,
            IAuditLogRepository auditLogService)
        {
            _brandRepository = brandRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
        }

        public async Task<Result<Guid>> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var brand = await _brandRepository.GetByIdAsync(dto.Id);
            if (brand is null)
                return Result<Guid>.Failure("Brand not found.");

            var changes = new List<string>();

            if (!string.IsNullOrWhiteSpace(dto.Name) && !dto.Name.Equals(brand.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (await _brandRepository.ExistsByNameAsync(dto.Name))
                    return Result<Guid>.Failure("Another brand with this name already exists.");

                brand.UpdateName(dto.Name);
                changes.Add($"Name: {dto.Name}");
            }

            if (!string.IsNullOrWhiteSpace(dto.WebsiteUrl) && dto.WebsiteUrl != brand.WebsiteUrl)
            {
                brand.UpdateWebsiteUrl(dto.WebsiteUrl);
                changes.Add($"WebsiteUrl: {dto.WebsiteUrl}");
            }

            if (!string.IsNullOrWhiteSpace(dto.LogoUrl) && dto.LogoUrl != brand.LogoUrl)
            {
                brand.UpdateLogoUrl(dto.LogoUrl);
                changes.Add($"LogoUrl: {dto.LogoUrl}");
            }

            if (dto.Description != null && dto.Description != brand.Description)
            {
                brand.UpdateDescription(dto.Description);
                changes.Add($"Description: {dto.Description}");
            }

            if (changes.Count == 0)
                return Result<Guid>.Failure("No changes detected.");

            await _unitOfWork.BeginTransactionAsync();
            _brandRepository.Update(brand);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.AddAsync(new AuditLog(
                userId: _authService.CurrentUser()!.Id,
                action: "Update Brand",
                entityName: nameof(Brand),
                entityId: brand.Id,
                details: $"Fields updated: {string.Join(", ", changes)}",
                ip: request.RequestMetadata.IpAddress,
                userAgent: request.RequestMetadata.UserAgent
            ));

            return Result<Guid>.Success(brand.Id, "Brand updated successfully.");
        }
    }

}
