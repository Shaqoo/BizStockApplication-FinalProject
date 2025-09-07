using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.User
{
    using Application.Dto.RequestModels;
    using FluentValidation;
    using Microsoft.AspNetCore.Http;

    public class UpdateProfilePictureDtoValidator : AbstractValidator<UpdateProfilePictureDto>
    {
        public UpdateProfilePictureDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Profile picture is required.")
                .Must(BeAnImage).WithMessage("Only image files are allowed (jpeg, jpg, png, gif, bmp, webp, svg).")
                .Must(f => f.Length <= 5 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 5MB.");
        }

        private bool BeAnImage(IFormFile file)
        {
            if (file == null) return false;

            var allowedContentTypes = new[]
            {
            "image/jpeg",
            "image/jpg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/webp",
            "image/svg+xml"
        };

            return allowedContentTypes.Contains(file.ContentType.ToLower());
        }
    }

}
