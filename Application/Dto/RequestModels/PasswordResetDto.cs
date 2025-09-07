namespace Application.Dto.RequestModels
{
    public record PasswordResetDto
    {
        public required string Email { get; set; }
        public required string NewPassword { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
