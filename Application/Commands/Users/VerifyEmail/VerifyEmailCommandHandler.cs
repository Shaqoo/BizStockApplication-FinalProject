using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;

namespace Application.Commands.Users.VerifyEmail
{
    public class VerifyEmailCommandHandler
        : IRequestHandler<VerifyEmailCommand, Result<string>>
    {
        private readonly IMemoryCacheService _cache;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VerifyEmailCommandHandler(
            IMemoryCacheService cache,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _userRepository = userRepository;
        }

        public async Task<Result<string>> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"email_verification:{request.Email}";

            var storedToken = await _cache.GetAsync<string>(cacheKey);

            if (storedToken == null)
                return Result<string>.Failure("Verification token expired or not found.");

            if (storedToken != request.Token)
                return Result<string>.Failure("Invalid verification token.");

            
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Failure("User not found.");

            user.VerifyEmail();
            await _userRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _cache.RemoveAsync(cacheKey);

            return Result<string>.Success("Email verified successfully.");
        }
    }

}
