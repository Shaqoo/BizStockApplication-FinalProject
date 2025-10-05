using Application.Dto;
using Application.Pagination;
using Application.Queries.Invoices.GetInvoiceById;
using Application.Queries.Invoices.GetInvoices;
using Application.Queries.Invoices.GetInvoicesByCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get an invoice by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the invoice.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/invoice/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///
        /// Sample response (200 OK):
        /// ```json
        /// {
        ///   "isSuccess": true,
        ///   "data": {
        ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "invoiceNumber": "INV-2025-0001",
        ///     "customerId": "11111111-2222-3333-4444-555555555555",
        ///     "status": "Unpaid",
        ///     "subTotal": 15000,
        ///     "discount": 0,
        ///     "tax": 750,
        ///     "totalAmount": 15750,
        ///     "dueDate": "2025-10-15T00:00:00Z",
        ///     "items": [
        ///       {
        ///         "productId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
        ///         "description": "MacBook Pro 14-inch",
        ///         "quantity": 1,
        ///         "unitPrice": 15000,
        ///         "totalPrice": 15000
        ///       }
        ///     ],
        ///     "payments": []
        ///   }
        /// }
        /// ```
        /// </remarks>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<InvoiceDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetInvoiceByIdQuery(id));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Get paginated invoices for a specific customer.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="pageRequest">Pagination parameters (page number, page size).</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/invoice/customer/11111111-2222-3333-4444-555555555555?pageNumber=1&amp;pageSize=10
        ///
        /// Sample response (200 OK):
        /// ```json
        /// {
        ///   "isSuccess": true,
        ///   "data": {
        ///     "items": [
        ///       {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "invoiceNumber": "INV-2025-0002",
        ///         "status": "Paid",
        ///         "totalAmount": 5000,
        ///         "dueDate": "2025-09-28T00:00:00Z"
        ///       }
        ///     ],
        ///     "pageNumber": 1,
        ///     "pageSize": 10,
        ///     "totalCount": 1,
        ///     "totalPages": 1
        ///   }
        /// }
        /// ```
        /// </remarks>
        [HttpGet("customer/{customerId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<PaginatedList<InvoiceDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCustomerId(Guid customerId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetInvoicesByCustomerQuery(pageRequest, customerId));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Get all invoices (Admin only)
        /// </summary>
        /// <param name="pageRequest">Pagination request (page number, page size, etc.)</param>
        /// <returns>Paginated list of invoices</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedList<InvoiceDto>>), StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetInvoices([FromQuery] PageRequest pageRequest)
        {
            var query = new GetInvoicesQuery(pageRequest);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }

}
