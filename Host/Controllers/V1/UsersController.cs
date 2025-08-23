using Application.Commands.Users.AddBiometrics;
using Application.Commands.Users.ChangePassword;
using Application.Commands.Users.DeActivateUser;
using Application.Commands.Users.Login;
using Application.Commands.Users.LoginWithBioMetrics;
using Application.Commands.Users.LossAccountRequest;
using Application.Commands.Users.RecoveryLogin;
using Application.Commands.Users.RefreshToken;
using Application.Commands.Users.RegenerateMfa;
using Application.Commands.Users.RequestChangePassword;
using Application.Commands.Users.RequestPasswordChange;
using Application.Commands.Users.ResetPassword;
using Application.Commands.Users.SendEmailVerificationToken;
using Application.Commands.Users.UpdateLostAccessRequest;
using Application.Commands.Users.UpdateName;
using Application.Commands.Users.UpdateProfilePicture;
using Application.Commands.Users.VerifyEmail;
using Application.Commands.Users.VerifyMfa;
using Application.Commands.Users.VerifyPassword;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Interfaces.Service;
using Application.Pagination;
using Application.Queries.Users.GetAllUsers;
using Application.Queries.Users.GetByEmail;
using Application.Queries.Users.GetById;
using Application.Queries.Users.GetLostAccessRequests;
using Application.Queries.Users.GetMyProfile;
using Application.Queries.Users.GetUserStats;
using Application.Queries.Users.SearchBykeyword;
using Fido2NetLib;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFidoCredentialService _fidoService;

        public UsersController(IMediator mediator, IFidoCredentialService fidoCredentialService)
        {
            _mediator = mediator;
            _fidoService = fidoCredentialService;
        }

        /// <summary>Get user by ID</summary>
        /// <param name="userId">User's GUID</param>
        /// <returns>User details</returns>
        /// <response code="200">User found</response>
        /// <response code="404">User not found</response>
        [Authorize]
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById([FromRoute] Guid userId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(userId), cancellationToken);

            if (!result.IsSuccess || result.Data is null)
                return NotFound(result.Message);

            return Ok(result);
        }

        /// <summary>Get user by email</summary>
        /// <param name="email">User's email</param>
        /// <returns>User details</returns>
        /// <response code="200">User found</response>
        /// <response code="404">User not found</response>
        [Authorize]
        [HttpGet("by-email/{email}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserByEmail([FromRoute] string email, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserByEmailQuery(email), cancellationToken);

            if (!result.IsSuccess || result.Data is null)
                return NotFound(result.Message);

            return Ok(result);
        }

        /// <summary>Get the currently authenticated user</summary>
        /// <returns>Current user's profile</returns>
        /// <response code="200">User found</response>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMyProfileQuery(), cancellationToken);
            return Ok(result);
        }

        /// <summary>Search for users</summary>
        /// <param name="query">Search keyword</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged list of users</returns>
        /// <response code="200">Users found</response>
        [Authorize]
        [HttpGet("search")]
        [ProducesResponseType(typeof(PaginatedList<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchUsers([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new SearchUsersByKeywordQuery(new PageRequest { Page = page, PageSize = pageSize }, query), cancellationToken);
            return Ok(result.Data);
        }


        /// <summary>Get all users with pagination</summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged list of users</returns>
        /// <response code="200">Users fetched successfully</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedList<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllUsersQuery(new PageRequest { Page = page, PageSize = pageSize }), cancellationToken);
            return Ok(result);
        }


        /// <summary>Get summary statistics about users</summary>
        /// <returns>User statistics</returns>
        /// <response code="200">Statistics fetched</response>
        [Authorize]
        [HttpGet("stats")]
        [ProducesResponseType(typeof(UserStatisticsDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserStats(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUserStatisticsQuery(), cancellationToken);
            return Ok(result);
        }


        /// <summary>
        /// Authenticates a user using email and password.
        /// </summary>
        /// <param name="model">Login credentials.</param>
        /// <returns>JWT token and user info on success, appropriate error on failure.</returns>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var result = await _mediator.Send(new LoginCommand(model, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
            {
                return result.Message switch
                {
                    "Invalid Credentials" => Unauthorized(result.Message),
                    "User is not active." => Unauthorized(result.Message),
                    var msg when msg.StartsWith("Account locked.") => Forbid(result.Message),
                    _ => BadRequest(result.Message)
                };
            }

            return Ok(result);
        }

        /// <summary>
        /// Generates FIDO2 biometric login options for a user.
        /// </summary>
        /// <returns>FIDO2 login challenge.</returns>
        [ProducesResponseType(typeof(AssertionOptions), StatusCodes.Status200OK)]
        [HttpPost("biometrics/generate-login-options")]
        public async Task<IActionResult> GenerateBiometricsLoginOptions([FromBody] string userIdentifier)
        {
            var options = await _fidoService.GenerateLoginOptionsAsync(userIdentifier);
            return Ok(options);
        }

        /// <summary>
        /// Verifies a biometric login assertion.
        /// </summary>
        /// <param name="request">Biometric login data from client.</param>
        /// <returns>JWT token and user info on success, appropriate error on failure.</returns>
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [HttpPost("biometrics/verify-login")]
        public async Task<IActionResult> VerifyLoginAssertion([FromBody] FingerprintLoginDto request)
        {
            var result = await _mediator.Send(new LoginWithBiometricsCommand(request, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
            {
                return result.Message switch
                {
                    "Invalid login request" => BadRequest(result.Message),
                    "User not allowed or account locked." => Forbid(result.Message),
                    _ => Unauthorized(result.Message)
                };
            }

            return Ok(result);
        }


        /// <summary>
        /// Generates FIDO2 biometric registration options for a user.
        /// </summary>
        /// <returns>FIDO2 registration challenge.</returns>
        [Authorize]
        [ProducesResponseType(typeof(CredentialCreateOptions), StatusCodes.Status200OK)]
        [HttpPost("biometrics/generate-registration-options")]
        public async Task<IActionResult> GenerateRegistrationOptions()
        {
            var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var options = await _fidoService.GenerateRegistrationOptionsAsync(Guid.Parse(userIdentifier));
            return Ok(options);
        }


        /// <summary>
        /// Verifies a biometric registration assertion and saves the credential.
        /// </summary>
        /// <param name="request">Biometric registration data.</param>
        /// <returns>Confirmation of successful registration or error.</returns>
        [Authorize]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost("biometrics/verify-registration")]
        public async Task<IActionResult> VerifyRegistration([FromBody] FingerprintRegistrationDto request)
        {
            var result = await _mediator.Send(new RegisterFingerprintCommand(request, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        /// <summary>
        /// Verifies a multi-factor authentication (MFA) code during login.
        /// </summary>
        /// <param name="dto">The MFA verification request model.</param>
        /// <returns>Returns a token if successful, or an error response.</returns>
        [Authorize]
        [HttpPost("verify-mfa")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> VerifyMfa([FromBody] VerifyMfaRequest dto)
        {
            var result = await _mediator.Send(new VerifyMfaCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return Unauthorized(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Updates the profile picture of the currently authenticated user.
        /// </summary>
        /// <param name="dto">The profile picture update model.</param>
        /// <returns>Returns success or error response.</returns>
        [Authorize]
        [HttpPut("profile-picture")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] UpdateProfilePictureDto dto)
        {
            var result = await _mediator.Send(new UpdateUserPictureCommand(dto,Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }


        /// <summary>
        /// Updates the full name of the currently authenticated user.
        /// </summary>
        /// <param name="dto">The full name update model.</param>
        /// <returns>Returns success or error response.</returns>
        [Authorize]
        [HttpPut("full-name")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateFullName([FromBody] UpdateNameDto dto)
        {
            var result = await _mediator.Send(new UpdateUserNameCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

       
        /// <summary>
        /// Deactivates a user account by ID.
        /// </summary>
        /// <param name="userId">The ID of the user to deactivate.</param>
        /// <returns>Status of the deactivation operation.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId:guid}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateUser([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new DeactivateUserCommand(Request.GetRequestMetadata(),userId));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }


        /// <summary>
        /// Validates a user's current password and initiates a password change request.
        /// This endpoint is used when the user knows their existing password and wants to change it.
        /// </summary>
        /// <param name="dto">The DTO containing the old new password.</param>
        /// <returns>Status of the password change request validation.</returns>
        [Authorize]
        [HttpPost("request-change-password")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestChangePassword([FromBody] RequestChangePasswordDto dto)
        {
            var result = await _mediator.Send(new RequestChangePasswordCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }


        /// <summary>
        /// Completes the password reset process by setting a new password 
        /// </summary>
        /// <param name="dto">
        /// The DTO containing:
        /// - <c>NewPassword</c>: The new password the user wants to set.
        /// - <c>ConfirmPassword</c>: A confirmation of the new password to ensure accuracy.
        /// </param>
        /// <returns>
        /// A result indicating whether the password was successfully changed.
        /// </returns>

        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest dto)
        {
            var result = await _mediator.Send(new ChangeUserPasswordCommand(dto, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }


        /// <summary>
        /// Sends a verification code to the user's email to initiate a password reset.
        /// </summary>
        /// <param name="passwordRequest">
        /// The request model containing:
        /// - <c>Email</c>: The registered email address of the user requesting the password change.
        /// </param>
        /// <returns>
        /// Returns a success message if the verification code was sent successfully; otherwise, returns an error message.
        /// </returns>
        [HttpPost("request-password-change")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestPasswordChange([FromBody] RequestPasswordRequest passwordRequest)
        {
            var result = await _mediator.Send(new RequestPasswordCommand(passwordRequest, Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Verifies the password reset code sent to the user's email.
        /// </summary>
        /// <param name="dto">
        /// The DTO containing:
        /// - <c>Email</c>: The registered email address of the user.
        /// - <c>Code</c>: The verification code sent to the user's email for password reset.
        /// </param>
        /// <returns>
        /// Returns a success message if the code is valid; otherwise, returns an error message.
        /// </returns>
        [HttpPost("verify-password-code")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyPasswordCode([FromBody] VerifyPasswordReset dto)
        {
            var result = await _mediator.Send(new VerifyPasswordResetCodeCommand(dto));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Resets a user's password after verifying the password reset code.
        /// </summary>
        /// <param name="dto">
        /// The DTO containing:
        /// - <c>Email</c>: The registered email address of the user.
        /// - <c>NewPassword</c>: The new password the user wants to set.
        /// - <c>ConfirmPassword</c>: A confirmation of the new password to ensure accuracy.
        /// </param>
        /// <returns>
        /// Returns a success message if the password reset is successful; otherwise, returns an error message.
        /// </returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto dto)
        {
            var result = await _mediator.Send(new PasswordResetCommand(dto));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Generates a new access token and refresh token pair using a valid refresh token.
        /// </summary>
        /// <param name="dto">
        /// The DTO containing:
        /// - <c>RefreshToken</c>: The refresh token previously issued to the user.
        /// </param>
        /// <returns>
        /// A new access token and refresh token if the request is valid; otherwise, an error message.
        /// </returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(dto));
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }



        /// <summary>
        /// Logs in a user using a recovery code instead of two-factor authentication.
        /// </summary>
        /// <param name="request">Contains the temporary token and the recovery code issued to the user.</param>
        /// <returns>Returns an <see cref="AuthDto"/> containing access and refresh tokens if successful.</returns>
        /// <response code="200">Login successful. Returns access and refresh tokens.</response>
        /// <response code="400">Invalid request payload or invalid/used recovery code.</response>
        [Authorize]
        [HttpPost("recovery-login")]
        [ProducesResponseType(typeof(AuthDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> RecoveryLogin([FromBody] RecoveryLoginRequest request)
        {
            var command = new RecoveryLoginCommand(request, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }


        /// <summary>
        /// Regenerates a user's MFA and recovery codes.
        /// </summary>
        /// <remarks>
        /// This action should only be performed by authenticated users who suspect that 
        /// their MFA device or recovery codes have been lost or compromised. 
        /// <br/>
        /// <br/>
        /// <b>⚠ Security Warning:</b> Once regenerated, old recovery codes immediately become invalid.
        /// </remarks>
        /// <param >The <c>RegenerateMfaCommand</c> containing the user ID and request metadata.</param>
        /// <returns>A response containing the new recovery codes.</returns>
        /// <response code="200">Returns the newly generated recovery codes for the authenticated user.</response>
        /// <response code="401">If the user is not authenticated.</response>
        /// <response code="403">If the user is not authorized to perform this action.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [Authorize]
        [HttpPost("regenerate")]
        [ProducesResponseType(typeof(Result<TwoFactorSetupDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegenerateMfa()
        {
            var command = new RegenerateMfaCommand(Request.GetRequestMetadata());
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Submits a lost access request when a user cannot access their account.
        /// </summary>
        /// <param name="dto">The details of the lost access request, including alternate contact info and problem description.</param>
        /// <returns>
        /// Returns the unique identifier of the created lost access request if successful, 
        /// or an error message if validation fails.
        /// </returns>
        /// <response code="200">Lost access request created successfully</response>
        /// <response code="400">Bad request - validation failed or a pending request already exists</response>
        /// <response code="404">User not found with the provided identifier</response>
        [HttpPost("lost-access-request")]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Result<Guid>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateLostAccessRequest([FromBody] CreateLostAccessRequestDto dto)
        {
            var command = new CreateLostAccessRequestCommand(dto,Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                if (result.Message!.Contains("No account found"))
                    return NotFound(result);

                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Updates the status and admin notes of an existing lost access recovery request.
        /// </summary>
        /// <param name="requestId">The unique identifier of the lost access request to update.</param>
        /// <param name="dto">The updated details for the lost access request, including status and admin notes.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with the updated request if successful;  
        /// otherwise returns <see cref="BadRequestObjectResult"/> with error details.
        /// </returns>
        [Authorize]
        [HttpPut("Update-Recovery-Access/{requestId:guid}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateLostAccessRequest(
            [FromRoute] Guid requestId,
            [FromBody] UpdateLostAccessRequestDto dto,
            CancellationToken cancellationToken)
        {
            var command = new UpdateLostAccessRequestCommand
            {
                RequestId = requestId,
                Dto = dto
            };

            var result = await _mediator.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }


        /// <summary>
        /// Get all pending lost access requests with pagination.
        /// </summary>
        /// <param name="pageNumber">Page number (starting from 1).</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Paginated list of lost access requests</returns>
        [HttpGet("pending-request")]
        [ProducesResponseType(typeof(Result<PaginatedList<LostAccessRequestDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPendingRequests(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetPendingUserLostAccessRequestsQuery(
                new PageRequest { Page = pageNumber, PageSize = pageSize });

            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        /// <summary>
        /// Sends an email verification token to the user's registered email.
        /// </summary>
        /// <param name="command">The command containing the user ID.</param>
        /// <returns>A result indicating whether the email was sent successfully.</returns>
        [HttpPost("send-email-verification")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendEmailVerification([FromBody] SendEmailVerificationTokenCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Verifies a user's email using the token sent to their email.
        /// </summary>
        /// <param name="command">The command containing the user ID and token.</param>
        /// <returns>A result indicating whether the verification was successful.</returns>
        [HttpPost("verify-email")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}