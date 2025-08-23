using Application.Configurations;
using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using System.Security.Cryptography;

namespace Application.Commands.Users.ResetPassword
{
    public class PasswordResetHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMemoryCacheService distributedCacheService
) : IRequestHandler<PasswordResetCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(PasswordResetCommand request, CancellationToken cancellationToken)
        {
            if (request.PasswordResetDto is null)
                return Result<string>.Failure("Password reset data is required.");

            var user = await userRepository.GetByEmailAsync(request.PasswordResetDto.Email);
            if (user is null)
                return Result<string>.Failure("User not found.");

             
            var verifiedKey = $"PasswordChangeVerified:{user.Id}";
            var isVerified = await distributedCacheService.GetAsync<bool>(verifiedKey);

            if (!isVerified)
                return Result<string>.Failure("Password reset not verified or code expired.");

             
            await distributedCacheService.RemoveAsync(verifiedKey);

             
            var salt = RandomNumberGenerator.GetBytes(64);
            string base64Salt = Convert.ToBase64String(salt);
            var hashedPassword = PasswordHasher.HashPassword(request.PasswordResetDto.NewPassword, base64Salt);
            user.ChangePassword(hashedPassword, base64Salt);

             
            try
            {
                await unitOfWork.BeginTransactionAsync();
                await userRepository.UpdateUserAsync(user);
                await unitOfWork.CommitTransactionAsync();
                return Result<string>.Success("Password reset successfully.");
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<string>.Failure($"An error occurred while resetting the password: {ex.Message}");
            }
        }
    }

}
