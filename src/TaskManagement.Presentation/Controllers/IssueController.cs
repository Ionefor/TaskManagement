using Microsoft.AspNetCore.Authorization;
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
using TaskManagement.Application.Models;
using TaskManagement.Presentation.Requests.Issues;

namespace TaskManagement.Presentation.Controllers;

[Authorize]
[Route("issues")]
public class IssueController : ApplicationController
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
            return BadRequest(Envelope.Error(result.Error));

        return Created("", Envelope.Ok(result.Value));
    }
    
    [HttpDelete("{issueId:guid}")]
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
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{issueId:guid}/assignee")]
    public async Task<ActionResult<Guid>> SetAssignee(
        [FromRoute] Guid issueId,
        [FromServices] SetAssigneeHandler handler,
        [FromBody] SetAssigneeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{issueId:guid}/title")]
    public async Task<ActionResult<Guid>> UpdateTitle(
        [FromRoute] Guid issueId,
        [FromServices] UpdateTitleHandler handler,
        [FromBody] UpdateTitleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{issueId:guid}/description")]
    public async Task<ActionResult<Guid>> UpdateDescription(
        [FromRoute] Guid issueId,
        [FromServices] UpdateDescriptionHandler handler,
        [FromBody] UpdateDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
                request.ToCommand(issueId),
                cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{issueId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdateStatus(
        [FromRoute] Guid issueId,
        [FromServices] UpdateStatusHandler handler,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
                request.ToCommand(issueId),
                cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{issueId:guid}/priority")]
    public async Task<ActionResult<Guid>> UpdatePriority(
        [FromRoute] Guid issueId,
        [FromServices] UpdatePriorityHandler handler,
        [FromBody] UpdatePriorityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
                request.ToCommand(issueId),
                cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
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

        return Ok(Envelope.Ok(response));
    }
    
    [HttpGet("{issueId:guid}")]
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
            return BadRequest(Envelope.Error(result.Error));
        
        return Ok(Envelope.Ok(result.Value));
    }
}