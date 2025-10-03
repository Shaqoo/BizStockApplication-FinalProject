using Application.Commands.Payments.InitiatePayment;
using Application.Commands.Payments.VerifyPayment;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Payments.GetAllPaymentsPaged;
using Application.Queries.Payments.GetPaymentsByCustomer;
using Host.Extensions;
using Infrastructures.Settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<PaymentsController> logger;

        public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
        {
            this.mediator = mediator;
            this.logger = logger;
        }

        /// <summary>
        /// Initiates a new payment transaction via Paystack.
        /// </summary>
        /// <param name="request">The payment initiation request model.</param>
        /// <returns>Returns a URL where the user can complete the payment.</returns>
        [Authorize]
        [HttpPost("initiate")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InitiatePayment([FromBody] InitiatePaymentRequest request)
        {
            var command = new InitiatePaymentCommand(request,Request.GetRequestMetadata());
            var result = await mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        /// <summary>
        /// Verifies the status of a payment transaction.
        /// </summary>
        /// <param name="reference">The payment reference string.</param>
        /// <returns>Returns true if payment is successful, otherwise false.</returns>
       // [Authorize]
        [HttpPost("verify")]
        [ProducesResponseType(typeof(Result<PaystackVerifyResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<PaystackVerifyResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyPayment([FromQuery] string reference)
        {
            var command = new VerifyPaymentCommand(reference,Request.GetRequestMetadata());
            var result = await mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        /// <summary>
        /// Webhook endpoint for Paystack to notify about transaction updates.
        /// </summary>
        /// <remarks>
        /// This endpoint is called by Paystack after a user completes payment.
        /// Always verify the event with Paystack before updating your database.
        /// </remarks>
        /// <param name="paystackOptions">The raw Paystack event payload.</param>
        /// <returns>Returns 200 OK if processed successfully.</returns>
        [HttpPost("webhook")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Webhook([FromServices] IOptions<PaystackSettings> paystackOptions)
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                var secret = paystackOptions.Value.SecretKey;

                if (!Request.Headers.TryGetValue("X-Paystack-Signature", out var signature))
                {
                    logger.LogWarning("Webhook received without signature.");
                    return Ok();
                }

                using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
                var hash = BitConverter
                    .ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(body)))
                    .Replace("-", "")
                    .ToLower();

                if (!string.Equals(signature, hash, StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogWarning("Invalid Paystack webhook signature.");
                    return Ok();
                }

                var payload = JsonDocument.Parse(body);
                var reference = payload.RootElement
                    .GetProperty("data")
                    .GetProperty("reference")
                    .GetString();

                if (string.IsNullOrEmpty(reference))
                {
                    logger.LogWarning("Webhook received with no reference.");
                    return Ok();
                }

                var command = new VerifyPaymentCommand(reference,Request.GetRequestMetadata());
                await mediator.Send(command);

                logger.LogInformation("Webhook processed successfully for reference {Reference}", reference);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing webhook");
                return Ok(); 
            }
        }

        /// <summary>
        /// Get all payments for the currently authenticated customer. Supports pagination.
        /// </summary>
        /// <param name="page">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        [HttpGet]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(PaginatedList<PaymentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerPayments([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var customerId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var query = new GetPaymentsByCustomerQuery(customerId, new PageRequest { Page = page,PageSize = pageSize});
            var result = await mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result.Data);
        }

        /// <summary>
        /// Get all payments in the system (admin only). Supports pagination.
        /// </summary>
        /// <param name="page">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        [HttpGet("all")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(PaginatedList<PaymentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPayments([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var query = new GetAllPaymentsPagedQuery(new PageRequest { Page = page,PageSize = pageSize});
            var result = await mediator.Send(query);
            return Ok(result);
        }

    }

}
