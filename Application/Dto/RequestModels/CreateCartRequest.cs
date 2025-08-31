namespace Application.Dto.RequestModels
{
    public class CreateCartRequest
    {
        /// <summary>
        /// The user that owns the cart (if authenticated).
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// The guest session identifier (if user is not logged in).
        /// Required when UserId is null.
        /// </summary>
        public string? SessionId { get; set; }
    }

}
