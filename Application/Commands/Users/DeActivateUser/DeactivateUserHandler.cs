using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.DeActivateUser
{
    internal class DeactivateUserHandler(
    IUserRepository userRepository,
    IAuthService currentUser,
    IUnitOfWork unitOfWork,
    IAuditLogRepository auditLogRepository,
    IMemoryCacheService memoryCacheService
) : IRequestHandler<DeactivateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var current = currentUser.CurrentUser();

            var user = await userRepository.GetByIdAsync(request.Id);
            if (user is null || current is null)
            {
                return Result<string>.Failure("User not found.");
            }

             
          
            if (current.RoleName != "Admin")
            {
                return Result<string>.Failure("You are not authorized to deactivate this user.");
            }

            user.ToogleDelete();
            await userRepository.UpdateUserAsync(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "DeactivateUser",
                "User",
                user.Id,
                $"User {user.Email} was deactivated.",
                request.RequestMetadata.IpAddress,
                request.RequestMetadata.UserAgent
            ));
            await memoryCacheService.RemoveCacheByPrefix("SearchUser", "GetUserById", "GetUserByEmail", "AllUser");
            return Result<string>.Success("User account has been deactivated.");
        }
    }

}
