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

namespace Application.Commands.Brands.Create
{
    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Result<Guid>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogRepository _auditLogService;
        private readonly IAuthService _authService;

        public CreateBrandCommandHandler(
            IBrandRepository brandRepository,
            IUnitOfWork unitOfWork,
            IAuditLogRepository auditLogService,
            IAuthService authService)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _auditLogService = auditLogService;
            _authService = authService;
        }

        public async Task<Result<Guid>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (await _brandRepository.ExistsByNameAsync(dto.Name))
            {
                return Result<Guid>.Failure("A brand with the same name already exists.");
            }

            var brand = new Brand(dto.Name, dto.WebsiteUrl, dto.LogoUrl, dto.Description);

            await _unitOfWork.BeginTransactionAsync();
            await _brandRepository.AddAsync(brand);
            await _unitOfWork.CommitTransactionAsync();

            var log = new AuditLog(
                userId: _authService.CurrentUser()!.Id,
                action: "Create Brand",
                entityName: nameof(Brand),
                entityId: brand.Id,
                details: $"Brand '{dto.Name}' created.",
                ip: request.RequestMetadata.IpAddress,
                userAgent: request.RequestMetadata.UserAgent
            );
            await _auditLogService.AddAsync(log);

            return Result<Guid>.Success(brand.Id, "Brand created successfully.");
        }
    }

}
