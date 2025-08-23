using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.User
{
    using Application.Dto.RequestModels;
    using FluentValidation;
    using System.IdentityModel.Tokens.Jwt;

    public class TokenRequestModelValidator : AbstractValidator<TokenRequestModel>
    {
        public TokenRequestModelValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("Access token is required.")
                .Must(BeAValidJwt).WithMessage("Invalid access token format.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }

        private bool BeAValidJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.CanReadToken(token);
        }
    }

}
