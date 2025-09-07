using Application.Configurations;
using Application.Dto;
using Application.Extensions;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Application.Interfaces.UnitOfWork;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Commands.Users.Login
{
    public class LoginHandler(IUserRepository userRepository,
       IAuthService authService,
       ICaptchaService captchaService,
       IUnitOfWork unitOfWork,
       IMediator mediator,
       IAuditLogRepository auditLogRepository) : IRequestHandler<LoginCommand, Result<object>>
    {
        public async Task<Result<object>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.Model is null)
            {
                return Result<object>.Failure("Invalid request payload.");
            }
            var user = await userRepository.GetByEmailAsync(request.Model.Email);
            if (user == null)
            {
                return Result<object>.Failure("Invalid Credentials");
            }
            if (user.IsDeleted)
            {
                return Result<object>.Failure("User is not active.");
            }
            if(!user.IsEmailVerified)
            {
                return Result<object>.Failure("Unverified Email");
            }

            if (user.IsLockedOut)
            {

                var remaining = user.LockoutEnd!.Value - DateTime.UtcNow;
                await auditLogRepository.AddAsync(new AuditLog(
               user.Id,
               "LOGIN_ATTEMPT_LOCKED_OUT",
               "User",
               user.Id,
               "Attempted login while account is locked.",
               request.RequestMetadata.IpAddress,
               request.RequestMetadata.UserAgent
           ));
                return new Result<object>
                {
                    Data = new
                    {
                        remaining = new
                        {
                            minutes = remaining.Minutes,
                            seconds = remaining.Seconds
                        }
                    },
                    IsSuccess = false,
                    Message = $"Account locked. Try again in {remaining.Minutes}m {remaining.Seconds}s."
                };
            }
            if (user.RequiresCaptcha)
            {
                if (string.IsNullOrWhiteSpace(request.Model.CaptchaToken))
                {
                  
                    await auditLogRepository.AddAsync(new AuditLog(
                        user.Id,
                        "LOGIN_ATTEMPT_INVALID_CAPTCHA",
                        "User",
                        user.Id,
                        "Attempted login with invalid CAPTCHA.",
                        request.RequestMetadata.IpAddress,
                        request.RequestMetadata.UserAgent
                    ));

                    Console.WriteLine(request.Model.CaptchaToken);

                    bool validToken = false;
                    if (!string.IsNullOrEmpty(request.Model.CaptchaToken))
                    {
                        validToken = await captchaService.ValidateTokenAsync(request.Model.CaptchaToken);
                    }

                    Console.WriteLine(validToken);


                    return Result<object>.Failure("CAPTCHA validation failed.");
                }
            }

            var isValidPassword = PasswordHasher.VerifyPassword(user.Password, request.Model.Password, user.HashSalt);
            if (!isValidPassword)
            {
                await unitOfWork.BeginTransactionAsync();
                user.RegisterFailedLogin();
                 
                if (user.IsLockedOut)
                {
                    await mediator.Publish(new AccountLockedEvent(user.FullName, (string)user.Email, request.RequestMetadata.IpAddress, request.RequestMetadata.UserAgent), cancellationToken);
                }

                await userRepository.UpdateUserAsync(user);
                await unitOfWork.CommitTransactionAsync();

                await auditLogRepository.AddAsync(new AuditLog(
                    user.Id,
                    "LOGIN_ATTEMPT_FAILED",
                    "User",
                    user.Id,
                    "Failed login attempt with invalid password.",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));
                return Result<object>.Failure("Invalid credentials.");
            }

            if(!user.IsTwoFactorEnabled)
            {
                await auditLogRepository.AddAsync(new AuditLog(
                    user.Id,
                    "LOGIN_ATTEMPT_SUCCESS",
                    "User",
                    user.Id,
                    "Successful login attempt without MFA.",
                    request.RequestMetadata.IpAddress,
                    request.RequestMetadata.UserAgent
                ));

                user.LatestLogin();
                await userRepository.UpdateUserAsync(user);
                var loginToken = authService.GenerateToken(user.UserAsDto());

                return Result<object>.Success(new
                {
                    Token = loginToken,
                    Status = "LOGIN_SUCCESS"
                });
            }

            await unitOfWork.BeginTransactionAsync();
            user.ResetLoginAttempts();
            await userRepository.UpdateUserAsync(user);
            await unitOfWork.CommitTransactionAsync();

            Console.WriteLine(user.Id.ToString());
            var token = authService.GenerateTempJwt(user.Id.ToString());

            await auditLogRepository.AddAsync(new AuditLog(
                user.Id,
                "LOGIN_ATTEMPT_SUCCESS",
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
