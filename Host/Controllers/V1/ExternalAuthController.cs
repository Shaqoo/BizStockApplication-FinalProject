using Application.Commands.ExternalLogin.Facebook;
using Application.Commands.ExternalLogin.GitHub;
using Application.Commands.ExternalLogin.Google;
using Application.Commands.ExternalLogin.Microsoft;
using Application.Dto;
using Application.Dto.RequestModels;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ExternalAuthController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Login with Facebook access token
        /// </summary>
        /// <remarks>
        /// Requires a valid Facebook access token obtained from the client.
        /// </remarks>
        /// <param name="dto">Facebook login data</param>
        /// <returns>Authenticated user token</returns>
        [HttpPost("facebook/login")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FacebookLogin([FromBody] ExternalLoginDto dto, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new FacebookLoginCommand(dto, Request.GetRequestMetadata()), cancellationToken);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Login with Google access token
        /// </summary>
        /// <remarks>
        /// Requires a valid Google access token obtained from the client.
        /// </remarks>
        /// <param name="dto">Google login data</param>
        /// <returns>Authenticated user token</returns>
        [HttpPost("google/login")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GoogleLogin([FromBody] ExternalLoginDto dto, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GoogleLoginCommand(dto, Request.GetRequestMetadata()), cancellationToken);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Login with GitHub access token
        /// </summary>
        /// <remarks>
        /// Requires a valid GitHub access token obtained from the client after OAuth login.
        /// </remarks>
        /// <param name="dto">GitHub login data</param>
        /// <returns>Authenticated user token</returns>
        [HttpPost("github/login")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GitHubLogin([FromBody] ExternalLoginDto dto, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GitHubLoginCommand(dto, Request.GetRequestMetadata()), cancellationToken);
            return result.ToActionResult(this);
        }

        /// <summary>
        /// Login with Microsoft access token
        /// </summary>
        /// <remarks>
        /// Requires a valid Microsoft access token obtained from the client after OAuth login.
        /// </remarks>
        /// <param name="dto">Microsoft login data</param>
        /// <returns>Authenticated user token</returns>
        [HttpPost("microsoft/login")]
        [ProducesResponseType(typeof(AuthDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MicrosoftLogin([FromBody] ExternalLoginDto dto, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new MicrosoftLoginCommand(dto, Request.GetRequestMetadata()), cancellationToken);
            return result.ToActionResult(this);
        }
    }

}
