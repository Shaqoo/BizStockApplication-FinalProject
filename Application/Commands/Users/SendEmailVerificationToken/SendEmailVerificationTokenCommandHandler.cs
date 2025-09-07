using Application.Dto;
using Application.Interfaces.Repository;
using Application.Interfaces.Service;
using Domain.Enums;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace Application.Commands.Users.SendEmailVerificationToken
{
    public class SendEmailVerificationTokenCommandHandler
        : IRequestHandler<SendEmailVerificationTokenCommand, Result<string>>
    {
        private readonly IMemoryCacheService _cache;
        private readonly IEmailNotificationService _emailService;
        private readonly IUserRepository _userRepository;

        public SendEmailVerificationTokenCommandHandler(
            IMemoryCacheService cache,
            [FromKeyedServices(EmailNotificationType.Mailjet)]IEmailNotificationService emailService,
            IUserRepository userRepository)
        {
            _cache = cache;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(SendEmailVerificationTokenCommand request, CancellationToken cancellationToken)
        {
            if(!await _userRepository.CheckIfExists(a => a.Email == new Email(request.Email)))
            {
                return Result<string>.Failure("Email Does Not Exist");
            }
            var token = GenerateSecureDigits(8);

            var cacheKey = $"email_verification:{request.Email}";
            await _cache.SetAsync(cacheKey,token, TimeSpan.FromMinutes(15));

            var verificationLink = $"http://localhost:5500/general/verifyemail.html?userMail={request.Email}&token={token}";

            var message = $@"
Hello,

Thank you for registering with us.  
To complete your registration, please verify your email address.  

Your verification token is: **{token}**

Alternatively, you can click the link below to verify directly:  
{verificationLink}

If you did not register on our platform, please ignore this email.  

Best regards,  
Your App Team
";

             
            await _emailService.SendEmailAsync(
                request.Email,
                "Verify your email address",
                message
            );

            return Result<string>.Success("Verification email sent successfully.");
        }

        private string GenerateSecureDigits(int count)
        {
            char[] digits = new char[count];
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[1];
                for (int i = 0; i < count; i++)
                {
                    do
                    {
                        rng.GetBytes(buffer);
                    } while (buffer[0] >= 250); 

                    digits[i] = (char)('0' + (buffer[0] % 10));
                }
            }

            return new string(digits);
        }
    }

}
