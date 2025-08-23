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

namespace Application.Commands.Users.UpdateName
{
    public class UpdateNameHandler(IAuthService authService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IAuditLogRepository logRepository,
        IMemoryCacheService memoryCacheService) : IRequestHandler<UpdateUserNameCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateUserNameCommand request, CancellationToken cancellationToken)
        {
            if (request.UpdateNameDto is null)
            {
                return Result<string>.Failure("Invalid request data");
            }

            var currentUser = authService.CurrentUser();
            if (currentUser == null)
            {
                return Result<string>.Failure("User not found\nLogin Before Update Can Be Done");
            }
            try
            {
                await unitOfWork.BeginTransactionAsync();
                var user = await userRepository.GetByIdAsync(currentUser.Id);
                if (user == null || user.IsDeleted || user.IsLockedOut)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return Result<string>.Failure("User not allowed or account locked.");
                }
                var fullname = $"{request.UpdateNameDto.Firstname.Trim()} {request.UpdateNameDto.Lastname.Trim()}".Trim();
                user.ChangeName(fullname);

                await userRepository.UpdateUserAsync(user);
                await unitOfWork.CommitTransactionAsync();
                await logRepository.AddAsync(new AuditLog(
                    user.Id,
                    "Update Name",
                    "User",
                    user.Id,
                    $"User {user.Id} updated their name to {user.FullName}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));
                await memoryCacheService.RemoveCacheByPrefix("SearchUser", "GetUserById", "GetUserByEmail", "AllUser");
                return Result<string>.Success("User name updated successfully");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                await logRepository.AddAsync(new AuditLog(
                    currentUser.Id,
                    "Update Name Failed",
                    "User",
                    currentUser.Id,
                    $"Failed to update user name: {ex.Message}",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));
                return Result<string>.Failure($"Error updating user name: {ex.Message}");
            }
        }
    }
}
