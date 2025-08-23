using Application.Dto.RequestModels;
using FluentValidation;

namespace Application.Validators.LostAccessRequest
{
    public class CreateLostAccessRequestValidator : AbstractValidator<CreateLostAccessRequestDto>
    {
        public CreateLostAccessRequestValidator()
        {
            RuleFor(x => x.UserIdentifier)
                .NotEmpty().WithMessage("User identifier is required.");

            RuleFor(x => x.AlternateEmail)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.AlternateEmail))
                .WithMessage("Alternate email must be a valid email address.");

            RuleFor(x => x.AlternatePhone)
                .Matches(@"^\+?[1-9]\d{1,14}$").When(x => !string.IsNullOrEmpty(x.AlternatePhone))
                .WithMessage("Alternate phone must be in E.164 format (e.g. +2348109094694).");

            RuleFor(x => x.ProblemDescription)
                .NotEmpty().WithMessage("Problem description is required.")
                .MinimumLength(10).WithMessage("Problem description must be at least 10 characters long.")
                .MaximumLength(1000).WithMessage("Problem description cannot exceed 1000 characters.");
        }
    }

    public class UpdateLostAccessRequestValidator : AbstractValidator<UpdateLostAccessRequestDto>
    {
        public UpdateLostAccessRequestValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status provided.");

            RuleFor(x => x.AdminNotes)
                .MaximumLength(1000).WithMessage("Admin notes cannot exceed 1000 characters.");
        }
    }

}
