namespace Application.Dto
{
    public record UserDto(Guid Id, string Email,string fullName,int age,string phoneNumber,DateTime Dob ,DateTime LastLoggedIn,string Role ,
        bool IsEmailVerified, bool IsTwoFactorEnabled, string? profilepicture = null);

}
