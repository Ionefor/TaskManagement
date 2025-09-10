using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Commands.Issue.CreateIssue;
using TaskManagement.Application.Features.Commands.Issue.DeleteIssue;
using TaskManagement.Application.Features.Commands.Issue.SetAssignee;
using TaskManagement.Application.Features.Commands.Issue.UpdateDescription;
using TaskManagement.Application.Features.Commands.Issue.UpdatePriority;
using TaskManagement.Application.Features.Commands.Issue.UpdateStatus;
using TaskManagement.Application.Features.Commands.Issue.UpdateTitle;
using TaskManagement.Application.Features.Queries.Issue.GetIssueById;
using TaskManagement.Application.Features.Queries.Issue.GetIssuesWithPagination;
using TaskManagement.Presentation.Requests.Issues;

namespace TaskManagement.Presentation.Controllers;

[ApiController]
[Route("issues")]
public class IssueController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateIssueHandler handler,
        [FromBody] CreateIssueRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Created("", result.Value);
    }
    
    [HttpDelete("{issueId}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteIssueHandler handler,
        [FromRoute] Guid issueId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteIssueCommand(issueId);
        
        var result = await handler.Handle(
            command,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPut("{issueId}/assignee")]
    public async Task<ActionResult<Guid>> SetAssignee(
        [FromRoute] Guid issueId,
        [FromServices] SetAssigneeHandler handler,
        [FromForm] SetAssigneeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPut("{issueId}/title")]
    public async Task<ActionResult<Guid>> UpdateTitle(
        [FromRoute] Guid issueId,
        [FromServices] UpdateTitleHandler handler,
        [FromForm] UpdateTitleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPut("{issueId}/description")]
    public async Task<ActionResult<Guid>> UpdateDescription(
        [FromRoute] Guid issueId,
        [FromServices] UpdateDescriptionHandler handler,
        [FromForm] UpdateDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
                request.ToCommand(issueId),
                cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPut("{issueId}/status")]
    public async Task<ActionResult<Guid>> UpdateStatus(
        [FromRoute] Guid issueId,
        [FromServices] UpdateStatusHandler handler,
        [FromForm] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
                request.ToCommand(issueId),
                cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPut("{issueId}/priority")]
    public async Task<ActionResult<Guid>> UpdatePriority(
        [FromRoute] Guid issueId,
        [FromServices] UpdatePriorityHandler handler,
        [FromForm] UpdatePriorityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
                request.ToCommand(issueId),
                cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
    
    [HttpGet]
    public async Task<ActionResult<Guid>> GetAll(
        [FromServices] GetIssuesWithPaginationHandler handler,
        [FromQuery] GetIssuesWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(
                request.ToQuery(),
                cancellationToken);

        return Ok(response);
    }
    
    [HttpGet("{issueId}")]
    public async Task<ActionResult<Guid>> GetById(
        [FromServices] GetIssueByIdHandler handler,
        [FromRoute] Guid issueId,
        CancellationToken cancellationToken)
    {
        var query = new GetIssueByIdQuery(issueId);
        
        var result = await handler.Handle(
            query,
            cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}