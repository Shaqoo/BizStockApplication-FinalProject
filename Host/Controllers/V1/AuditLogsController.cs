using Application.Pagination;
using Application.Queries.AuditLogs.GetAllLogs;
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
}
