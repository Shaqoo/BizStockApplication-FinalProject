using Application.Dto.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.ChatMessage
{
    public class SendMessageRequestValidator : AbstractValidator<SendMessageRequest>
    {
        public SendMessageRequestValidator()
        {
            RuleFor(x => x.ChatThreadId).NotEmpty();

            RuleFor(x => new { x.Message, x.Audio, x.Picture })
                .Must(x => !string.IsNullOrWhiteSpace(x.Message) || x.Audio != null || x.Picture != null)
                .WithMessage("At least one of Message, Audio, or Picture must be provided.");

            When(x => x.Audio != null, () =>
            {
                RuleFor(x => x.Audio!.ContentType)
                    .Must(ct => ct.StartsWith("audio/"))
                    .WithMessage("Audio must be a valid audio file.");

                RuleFor(x => x.Audio!.Length)
                    .LessThanOrEqualTo(5 * 1024 * 1024)  
                    .WithMessage("Audio file must not exceed 5MB.");
            });

            When(x => x.Picture != null, () =>
            {
                RuleFor(x => x.Picture!.ContentType)
                    .Must(ct => ct.StartsWith("image/"))
                    .WithMessage("Picture must be a valid image file.");

                RuleFor(x => x.Picture!.Length)
                    .LessThanOrEqualTo(3 * 1024 * 1024) 
                    .WithMessage("Picture must not exceed 3MB.");
            });
        }
    }

}
