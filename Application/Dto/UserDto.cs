namespace Application.Dto
{
    public record UserDto(Guid Id, string Email,string fullName,int age,string phoneNumber,DateTime Dob ,DateTime LastLoggedIn,string Role ,
        string Gender,bool IsEmailVerified, bool IsTwoFactorEnabled, string? profilepicture = null,bool isActive = true,bool IsFidoRegistered = false,int devices = 0);

}
