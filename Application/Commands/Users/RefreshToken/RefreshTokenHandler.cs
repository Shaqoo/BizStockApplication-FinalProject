using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Users.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
             
            var user = await _userRepository.GetByRfreshToken(request.TokenDto.RefreshToken);
            if (user is null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return Result<AuthDto>.Failure("Invalid or expired refresh token.");
            }

             
            var newAccessToken = _authService.GenerateToken(user.UserAsDto());  
             
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _userRepository.UpdateUserAsync(user);
                await _unitOfWork.CommitTransactionAsync();

                return Result<AuthDto>.Success(new AuthDto(newAccessToken,request.TokenDto.RefreshToken));
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return Result<AuthDto>.Failure($"Failed to refresh token: {ex.Message}");
            }
        }
    }

}
