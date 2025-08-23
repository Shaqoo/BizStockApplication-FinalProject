using Application.Configurations;
using Application.Dto.RequestModels;
using FluentValidation; 

namespace Application.Validators.ChatMessage
{
    public class ReactToMessageRequestValidator : AbstractValidator<ReactToMessageRequest>
    {
        public ReactToMessageRequestValidator()
        {
            RuleFor(x => x.Emoji)
                .NotEmpty()
                .Must(BeAValidEmoji).WithMessage("Emoji must be a valid emoji character.");
        }

         
        public static bool BeAValidEmoji(string input)
        {
            return typeof(Emoji)
                .GetFields()
                .Select(f => (string)f.GetRawConstantValue()!)
                .Any(val => val == input);
        }

    }
}
