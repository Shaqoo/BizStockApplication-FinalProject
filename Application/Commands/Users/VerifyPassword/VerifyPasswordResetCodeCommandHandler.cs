using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.VerifyPassword
{
    public class VerifyPasswordResetCodeCommandHandler
    : IRequestHandler<VerifyPasswordResetCodeCommand, Result<string>>
    {
        private readonly IMemoryCacheService _distributedCacheService;
        private readonly IUserRepository _userRepository;

        public VerifyPasswordResetCodeCommandHandler(IMemoryCacheService distributedCacheService,IUserRepository userRepository)
        {
            _distributedCacheService = distributedCacheService;
            _userRepository = userRepository;
        }

        public async Task<Result<string>> Handle(VerifyPasswordResetCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.VerifyPassword.Email);
            if (user == null)
            {
                return Result<string>.Failure("Email Not Found");
            }

            var cacheKey = $"PasswordChangeToken:{user.Id}";
            var token = await _distributedCacheService.GetAsync<string>(cacheKey);

            if (token is null || token != request.VerifyPassword.Code)
            {
                return Result<string>.Failure("Invalid or expired token.");
            }

            await _distributedCacheService.RemoveAsync(cacheKey);

            
            var verifiedKey = $"PasswordChangeVerified:{user.Id}";
            await _distributedCacheService.SetAsync(verifiedKey, true, TimeSpan.FromMinutes(5));

            return Result<string>.Success("Code verified successfully.");
        }
    }

}
