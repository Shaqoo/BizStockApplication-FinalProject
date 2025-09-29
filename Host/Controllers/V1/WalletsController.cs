using Application.Commands.Wallets.ChangeWalletPin;
using Application.Commands.Wallets.CreateWallet;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.Wallets.GetAllWalletTransactions;
using Application.Queries.Wallets.GetWalletByCustomerId;
using Application.Queries.Wallets.GetWalletTransactionsByWalletId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a wallet for a customer.
        /// </summary>
        /// <param name="request">The wallet creation request.</param>
        /// <returns>The ID of the created wallet.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletRequest request)
        {
            var result = await _mediator.Send(new CreateWalletCommand(request));
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Changes the PIN of an existing wallet.
        /// </summary>
        /// <param name="request">The change PIN request.</param>
        /// <returns>True if the operation succeeded.</returns>
        [HttpPut("change-pin")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeWalletPin([FromBody] ChangeWalletPinRequest request)
        {
            var result = await _mediator.Send(new ChangeWalletPinCommand(request));
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a wallet by customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>The wallet details.</returns>
        [HttpGet("by-customer/{customerId:guid}")]
        [ProducesResponseType(typeof(WalletDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWalletByCustomerId(Guid customerId)
        {
            var result = await _mediator.Send(new GetWalletByCustomerIdQuery(customerId));
            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        /// <summary>
        /// Get all wallet transactions for a specific wallet (customer only). Supports pagination.
        /// </summary>
        /// <param name="walletId">Wallet ID</param>
        /// <param name="page">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        [HttpGet("{walletId}/transactions")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(PaginatedList<WalletTransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWalletTransactionsByWalletId(
            Guid walletId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new GetWalletTransactionsByWalletIdQuery(walletId, new PageRequest {Page = page,PageSize = pageSize });
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result.Data);
        }

        /// <summary>
        /// Get all wallet transactions in the system (admin only). Supports pagination.
        /// </summary>
        /// <param name="page">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        [HttpGet("transactions/all")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(PaginatedList<WalletTransactionDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllWalletTransactions(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50)
        {
            var query = new GetAllWalletTransactionsPagedQuery(new PageRequest { Page = page, PageSize = pageSize });
            var result = await _mediator.Send(query);
            return Ok(result.Data);
        }
    }

}
