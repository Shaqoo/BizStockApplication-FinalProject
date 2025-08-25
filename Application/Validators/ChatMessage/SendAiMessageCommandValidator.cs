using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.ChatMessage
{
    public class SendAiMessageCommandValidator : AbstractValidator<SendAiMessageRequest>
    {
        public SendAiMessageCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question cannot be empty.")
                .MinimumLength(2).WithMessage("Question must be at least 3 characters long.");

            RuleFor(x => x.Role)
                .NotEmpty();
        }
    }

}
