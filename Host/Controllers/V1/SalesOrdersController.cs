using Application.Commands.SalesOrders.CancelOrder;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Pagination;
using Application.Queries.SalesOrders.GetOrderCostAndETA;
using Application.Queries.SalesOrders.GetSalesOrderById;
using Application.Queries.SalesOrders.GetSalesOrdersByCustomerId;
using Application.Queries.SalesOrders.GetSalesOrdersByUser;
using Application.Queries.SalesOrders.SearchOrders;
using Application.Queries.SalesOrders.TrackItem;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiversion}/[controller]")]
    public class SalesOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SalesOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get estimated order cost and delivery time for a given delivery address.
        /// </summary>
        [HttpGet("{deliveryAddressId:guid}/cost-eta")]
        public async Task<IActionResult> GetOrderCostAndETA(Guid deliveryAddressId)
        {
            var query = new GetOrderCostAndETAQuery(deliveryAddressId, Request.GetRequestMetadata());

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Get a sales order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the sales order.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/salesorder/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///
        /// Sample response (200 OK):
        /// ```json
        /// {
        ///   "isSuccess": true,
        ///   "data": {
        ///     "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///     "orderNumber": "SO-2025-0001",
        ///     "customerId": "11111111-2222-3333-4444-555555555555",
        ///     "status": "Pending",
        ///     "subTotal": 25000,
        ///     "discount": 0,
        ///     "tax": 1250,
        ///     "total": 26250,
        ///     "items": [
        ///       {
        ///         "productId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
        ///         "productName": "iPhone 14",
        ///         "quantity": 1,
        ///         "unitPrice": 25000,
        ///         "totalPrice": 25000
        ///       }
        ///     ]
        ///   }
        /// }
        /// ```
        /// </remarks>
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<SalesOrderDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetSalesOrderByIdQuery(id));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Track an order item by tracking number.
        /// </summary>
        /// <param name="trackingNumber">The tracking number for the item.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/salesorder/track/FEZ123456
        ///
        /// Sample response (200 OK):
        /// ```json
        /// {
        ///   "isSuccess": true,
        ///   "data": {
        ///     "trackingNumber": "FEZ123456",
        ///     "status": "InTransit",
        ///     "lastUpdated": "2025-10-02T08:30:00Z",
        ///     "location": "Lagos Hub"
        ///   }
        /// }
        /// ```
        /// </remarks>
        [HttpGet("track/{trackingNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<TrackOrderResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Track(string trackingNumber)
        {
            var result = await _mediator.Send(new TrackItemQuery(trackingNumber));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Get paginated sales orders for the current customer.
        /// </summary>
        /// <param name="pageRequest">Pagination parameters (page number, page size).</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/salesorder/customer?pageNumber=1&amp;pageSize=10
        ///
        /// Sample response (200 OK):
        /// ```json
        /// {
        ///   "isSuccess": true,
        ///   "data": {
        ///     "items": [
        ///       {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "orderNumber": "SO-2025-0001",
        ///         "status": "Dispatched",
        ///         "total": 15000
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
        [HttpGet("customer")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<PaginatedList<SalesOrderDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCurrentCustomer([FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetSalesOrdersByCustomerQuery(pageRequest));
            return Ok(result);
        }

        /// <summary>
        /// Get paginated sales orders for a specific customer by customerId.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <param name="pageRequest">Pagination parameters (page number, page size).</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/salesorder/customer/11111111-2222-3333-4444-555555555555?pageNumber=1&amp;pageSize=5
        ///
        /// Sample response (200 OK):
        /// ```json
        /// {
        ///   "isSuccess": true,
        ///   "data": {
        ///     "items": [
        ///       {
        ///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///         "orderNumber": "SO-2025-0002",
        ///         "status": "Pending",
        ///         "total": 5000
        ///       }
        ///     ],
        ///     "pageNumber": 1,
        ///     "pageSize": 5,
        ///     "totalCount": 1,
        ///     "totalPages": 1
        ///   }
        /// }
        /// ```
        /// </remarks>
        [HttpGet("customer/{customerId:guid}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Result<PaginatedList<SalesOrderDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCustomerId(Guid customerId, [FromQuery] PageRequest pageRequest)
        {
            var result = await _mediator.Send(new GetSalesOrderByCustomerIdQuery(customerId, pageRequest));
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        /// <summary>
        /// Cancels a specific sales order.
        /// </summary>
        /// <param name="salesOrderId">The unique identifier of the sales order to cancel.</param>
        /// <returns>Returns a success message if the order was canceled successfully.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/v1/SalesOrders/{salesOrderId}/cancel
        /// 
        /// </remarks>
        [HttpPost("{salesOrderId:guid}/cancel")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelSalesOrder(Guid salesOrderId)
        {
            var command = new CancelSalesOrderCommand(salesOrderId, Request.GetRequestMetadata());
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Searches sales orders by order number, customer email, or phone number.
        /// </summary>
        /// <param name="query">Search text (order number, email, or phone)</param>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Number of items per page (default: 10)</param>
        /// <returns>
        /// Paginated list of matching sales orders, including customer info and order items.
        /// </returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(Result<PaginatedList<SalesOrderDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<Unit>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchOrders(
            [FromQuery] string query,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var pageRequest = new PageRequest { Page = page, PageSize = pageSize };

            var result = await _mediator.Send(new SearchOrdersQuery(query, pageRequest));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

    }
}
