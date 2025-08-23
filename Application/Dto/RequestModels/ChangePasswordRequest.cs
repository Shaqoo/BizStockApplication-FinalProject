namespace Application.Dto.RequestModels
{
    public record ChangePasswordRequest(string newPassword, string confirmNewPassword);
}
