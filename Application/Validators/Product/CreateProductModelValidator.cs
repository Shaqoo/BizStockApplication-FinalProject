using Application.Dto.RequestModels;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validations.Product
{
    public class CreateProductModelValidator : AbstractValidator<CreateProductRequestModel>
    {
        public CreateProductModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Product Name is Required")
                .MaximumLength(50)
                .WithMessage("Product Name Cannot exceed 50 characters");

            RuleFor(x => x.SKU).NotEmpty()
                .WithMessage("SKU is Required")
                .Matches(@"^[A-Z0-9\-]+$")
                .WithMessage("SKU must Be Alpha-Numeric with dashes(e.g., ABC-123)");

            RuleFor(x => x.Barcode).NotEmpty()
                .WithMessage("Barcode Name is Required")
                .Matches(@"^\d{8,14}$")
                .WithMessage("Barcode Must Be A Numeric String Between 8 and 14 digits");

            RuleFor(x => x.QrCodeValue).NotEmpty()
                .WithMessage("QrCode is Required")
                .Must(BeAValidBase64OrUrl)
                .WithMessage("QrCode Must Be A Valid Base64 string or URL");

            RuleFor(x => x.ImageUrl)
                .NotNull().WithMessage("Product picture is required.")
                .Must(BeAnImage).WithMessage("Only image files are allowed (jpeg, jpg, png, gif, bmp, webp, svg).")
                .Must(f => f.Length <= 2 * 1024 * 1024)
                .WithMessage("Image size must be less than or equal to 2MB.");

            RuleFor(x => x.UnitOfMeasure)
                .IsInEnum();

            RuleFor(x => new { x.CostPrice, x.SellingPrice }).NotNull().NotEmpty()
                .Must(x => x.CostPrice >= 50 && x.SellingPrice >= 50)
                .WithMessage("Price Must Be Greater Than Or Equals To 50");

            RuleFor(x => x.SellingPrice).GreaterThanOrEqualTo(x => x.CostPrice)
            .WithMessage("Selling price must be greater than or equal to cost price.");
     


            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required.")
                .NotEqual(Guid.Empty).WithMessage("Category ID cannot be empty.");

            RuleFor(x => x.BrandId).NotEmpty()
                .WithMessage("Brand is required.")
                .NotEqual(Guid.Empty).WithMessage("Brand ID cannot be empty.");

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description).NotEmpty()
                    .Must(desc => desc.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length <= 2000)
                    .WithMessage("Description must not exceed 2000 words");
            });



        }

        private bool BeAValidBase64OrUrl(string qrCode)
        {
            return IsBase64String(qrCode) || Uri.TryCreate(qrCode,UriKind.Absolute,out _);
        }

        private bool IsBase64String(string base64)
        {
            if(string.IsNullOrWhiteSpace(base64)) return false;

            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);

            return Convert.TryFromBase64String(base64, buffer, out _);
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
