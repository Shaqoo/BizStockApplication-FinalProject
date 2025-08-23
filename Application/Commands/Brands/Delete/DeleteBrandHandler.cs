using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fido2NetLib.AuthenticatorAttestationRawResponse;

namespace Application.Commands.Brands.Delete
{
    public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result<string>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuditLogRepository _auditLogService;
        private readonly IAuthService _authService;

        public DeleteBrandCommandHandler(
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

        public async Task<Result<string>> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand is null)
                return Result<string>.Failure("Brand not found.");

            var hasProducts = await _brandRepository.HasProduct(brand.Id);
            if (hasProducts)
                return Result<string>.Failure("Cannot delete brand with associated products.");

            await _unitOfWork.BeginTransactionAsync();
            _brandRepository.Delete(brand);
            await _unitOfWork.CommitTransactionAsync();

            await _auditLogService.AddAsync(new AuditLog(
                userId: _authService.CurrentUser()!.Id,
                action: "Delete Brand",
                entityName: nameof(Brand),
                entityId: request.Id,
                details: $"Brand '{brand.Name}' deleted.",
                ip: request.RequestMetadata.IpAddress,
                userAgent: request.RequestMetadata.UserAgent
            ));

            return Result<string>.Success("Brand deleted successfully.");
        }
    }

}
