using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Users.UpdateProfilePicture
{
    public class UpdateUserPictureHandler(
    IUserRepository userRepository,
    IAuthService authService,
    IUnitOfWork unitOfWork,
    IAuditLogRepository auditLogRepository,
    IUploadService uploadService,
    IMemoryCacheService memoryCacheService)
    : IRequestHandler<UpdateUserPictureCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateUserPictureCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.UpdateProfilePicture is null || request.UpdateProfilePicture.File == null)
            {
                return Result<string>.Failure("Invalid request or missing image file.");
            }

            var currentUser = authService.CurrentUser();
            if (currentUser == null)
            {
                return Result<string>.Failure("Unauthorized: No authenticated user.");
            }

            var user = await userRepository.GetByEmailAsync(currentUser.Email);
            if (user == null)
            {
                return Result<string>.Failure("User not found.");
            }

            string url;
            try
            {
                using var stream = request.UpdateProfilePicture.File.OpenReadStream();
                url = await uploadService.UploadProfileImageAsync(stream, request.UpdateProfilePicture.File.FileName);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure("Image upload failed: " + ex.Message);
            }

            user.UpdateProfilePicture(url);

            try
            {
                await unitOfWork.BeginTransactionAsync();
                await userRepository.UpdateUserAsync(user);

                await unitOfWork.CommitTransactionAsync();

                await memoryCacheService.RemoveCacheByPrefix("SearchUser", "GetUserById", "GetUserByEmail", "AllUser");
                await auditLogRepository.AddAsync(new AuditLog(
                    userId: currentUser.Id,
                    action: "Update",
                    entityName: nameof(User),
                    entityId: user.Id,
                    details: $"Updated profile picture for user with email '{user.Email}'",
                    ip: request.RequestMetadata.IpAddress,
                    userAgent: request.RequestMetadata.UserAgent
                ));
            }
            catch (Exception ex)
            {

                await unitOfWork.RollbackTransactionAsync();
                await auditLogRepository.AddAsync(new AuditLog(
                    userId: currentUser.Id,
                    action: "Update",
                    entityName: nameof(User),
                    entityId: user.Id,
                    details: $"Failed to update profile picture for user with email '{user.Email}': {ex.Message}",
                    ip: request.RequestMetadata.IpAddress,
                    userAgent: request.RequestMetadata.UserAgent
                ));
                return Result<string>.Failure("Database update failed: " + ex.Message);
            }

            return Result<string>.Success(url, "Profile picture updated successfully.");
        }
    }

}
