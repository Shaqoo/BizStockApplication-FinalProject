using Application.Dto;
using Application.Pagination;
using Application.Queries.AuditLogs.GetAllLogs;
using Application.Queries.AuditLogs.GetLoginHeatMap;
using Application.Queries.AuditLogs.GetLogsBuUserId;
using Application.Queries.AuditLogs.GetLogsByAction;
using Application.Queries.AuditLogs.SearchLogs;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class AuditLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all audit logs (paginated)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<AuditLog>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetAllAuditLogsQuery(new PageRequest { Page = pageNumber, PageSize = pageSize});
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get audit logs by user id (paginated)
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(PaginatedList<AuditLog>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByUserId(Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetAuditLogsByUserIdQuery(userId, new PageRequest { Page = pageNumber, PageSize = pageSize });
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get audit logs by action (paginated)
    /// </summary>
    [HttpGet("by-action/{actionName}")]
    [ProducesResponseType(typeof(PaginatedList<AuditLog>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetByAction(string actionName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new GetAuditLogsByActionQuery(actionName, new PageRequest { Page = pageNumber, PageSize = pageSize });
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    /// <summary>
    /// Search audit logs (paginated)
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PaginatedList<AuditLog>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Search([FromQuery] string search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var query = new SearchAuditLogsQuery(search, new PageRequest { Page = pageNumber, PageSize = pageSize });
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Gets the login heatmap for the last 7 days.  
    /// Groups successful login attempts (including recovery logins) 
    /// into day-of-week and time ranges (Morning, Afternoon, Evening, Night).
    /// </summary>
    /// <remarks>
    /// This endpoint aggregates login success events from Elasticsearch 
    /// and returns counts in a format suitable for a heatmap chart.
    /// </remarks>
    /// <returns>
    /// A <see cref="LoginHeatmapDto"/> containing 7 days of login counts by time slot.
    /// </returns>
    /// <response code="200">Returns the heatmap data successfully.</response>
    /// <response code="500">If there was a server error while processing the request.</response>
    [HttpGet("login-heatmap")]
    [ProducesResponseType(typeof(LoginHeatmapDto), 200)]
    public async Task<IActionResult> GetLoginHeatMap()
    {
        var result = await _mediator.Send(new GetLoginHeatMapQuery());
        if (result.IsSuccess)
            return Ok(result);

        return StatusCode(500, result);
    }
}
