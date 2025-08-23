using Application.Configurations;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.LoginWithBioMetrics
{
    public class LoginWithBiometricsHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IFidoCredentialService fido2Service,
        IAuthService authService,
        IAuditLogRepository auditLogRepository) : IRequestHandler<LoginWithBiometricsCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(LoginWithBiometricsCommand request, CancellationToken cancellationToken)
        {
            if (request.LoginDto is null)
                return Result<object>.Failure("Invalid login request");

            var assertion = request.LoginDto.ToFido2Assertion();

            Guid userId;
            try
            {
                await unitOfWork.BeginTransactionAsync();
                userId = await fido2Service.VerifyAssertionAsync(assertion);
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<object>.Failure("Fingerprint verification failed: " + ex.Message);
            }

            var user = await userRepository.GetByIdAsync(userId);
            if (user is null || user.IsDeleted || user.IsLockedOut)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<object>.Failure("User not allowed or account locked.");
            }


            user.ResetLoginAttempts();
            await userRepository.UpdateUserAsync(user);
            await unitOfWork.CommitTransactionAsync();

            var token = authService.GenerateTempJwt(user.Id.ToString());

            await auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "Fingerprint Login",
                "User",
                user.Id,
                "Successful login attempt.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));

            return Result<object>.Success(new
            {
                Token = token,
                Status = "MFA_REQUIRED"
            });


        }
    }
}
