using Application.Commands.DeliveryAddresses.AddDeliveryAddress;
using Application.Commands.DeliveryAddresses.DeleteDeliveryAddress;
using Application.Commands.DeliveryAddresses.SetDefaultDeliveryAddress;
using Application.Commands.DeliveryAddresses.UpdateDeliveryAddress;
using Application.Dto;
using Application.Dto.RequestModels;
using Application.Queries.DeliveryAddresses.GetDefaultDeliveryAddress;
using Application.Queries.DeliveryAddresses.GetDeliveryAddressById;
using Application.Queries.DeliveryAddresses.GetDeliveryAddressesByCustomer;
using Application.Queries.DeliveryAddresses.HasDeliveryAddresses;
using Host.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [Authorize(Roles = "Customer")]
    public class DeliveryAddressesController : ControllerBase
    {
        private readonly IMediator mediator;

        public DeliveryAddressesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Creates a new delivery address for a customer.
        /// </summary>
        /// <param name="request">The delivery address details.</param>
        /// <returns>The ID of the created delivery address.</returns>
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDeliveryAddressRequest request)
        {
            var result = await mediator.Send(new CreateDeliveryAddressCommand(request,Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Updates an existing delivery address.
        /// </summary>
        /// <param name="request">The updated delivery address details.</param>
        /// <returns>True if the update was successful.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDeliveryAddressRequest request)
        {
            var result = await mediator.Send(new UpdateDeliveryAddressCommand(request,Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a delivery address by its ID.
        /// </summary>
        /// <param name="id">The delivery address ID.</param>
        /// <returns>True if the deletion was successful.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await mediator.Send(new DeleteDeliveryAddressCommand(id,Request.GetRequestMetadata()));
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Sets a delivery address as the default for the customer.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="addressId">The delivery address ID.</param>
        /// <returns>True if the operation was successful.</returns>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPatch("{customerId:guid}/default/{addressId:guid}")]
        public async Task<IActionResult> SetDefault(Guid customerId, Guid addressId)
        {
            var result = await mediator.Send(new SetDefaultDeliveryAddressCommand(customerId, addressId));
            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Get a delivery address by its unique identifier.
        /// </summary>
        /// <param name="addressId">The delivery address ID.</param>
        /// <returns>The delivery address details.</returns>
        [HttpGet("{addressId:guid}")]
        [ProducesResponseType(typeof(Result<DeliveryAddressDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid addressId)
        {
            var result = await mediator.Send(new GetDeliveryAddressByIdQuery(addressId));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Get all delivery addresses for a customer.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>A list of delivery addresses.</returns>
        [HttpGet("customer/{customerId:guid}")]
        [ProducesResponseType(typeof(Result<IEnumerable<DeliveryAddressDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCustomer(Guid customerId)
        {
            var result = await mediator.Send(new GetDeliveryAddressesByCustomerQuery(customerId));
            if (!result.IsSuccess || result.Data!.Any()) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Get the default delivery address for a customer.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>The default delivery address.</returns>
        [HttpGet("customer/{customerId:guid}/default")]
        [ProducesResponseType(typeof(Result<DeliveryAddressDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDefaultByCustomer(Guid customerId)
        {
            var result = await mediator.Send(new GetDefaultDeliveryAddressQuery(customerId));
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Checks Whether A Customer Has A Default Delivery Address.
        /// </summary>
        /// <returns>A list of delivery addresses.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> HasDefaultAddress(Guid customerId)
        {
            var result = await mediator.Send(new HasDeliveryAddressesQuery(customerId));
            return Ok(result);
        }
    }

}
