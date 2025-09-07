namespace Application.Dto.RequestModels
{
    public class RefreshTokenDto
    {
        /// <summary>
        /// The refresh token previously issued to the user.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
