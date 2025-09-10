using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.Features.Commands.SubIssue.CreateSubIssue;
using TaskManagement.Application.Features.Commands.SubIssue.DeleteSubIssue;
using TaskManagement.Application.Features.Commands.SubIssue.SetSubIssueAssignee;
using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueDescription;
using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssuePriority;
using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueStatus;
using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueTitle;
using TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesById;
using TaskManagement.Application.Features.Queries.SubIssue.GetSubIssuesByIssueIdWithPagination;
using TaskManagement.Application.Models;
using TaskManagement.Presentation.Requests.SubIssues;

namespace TaskManagement.Presentation.Controllers;

[Authorize]
[Route("issues/{issueId:guid}/subIssues")]
public class SubIssueController : ApplicationController
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromServices] CreateSubIssueHandler handler,
        [FromBody] CreateSubIssueRequest request,
        [FromRoute] Guid issueId,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Created("", Envelope.Ok(result.Value));
    }
    
    [HttpDelete("{subIssueId:guid}")]
    public async Task<ActionResult<Guid>> Delete(
        [FromServices] DeleteSubIssueHandler handler,
        [FromRoute] Guid issueId,
        [FromRoute] Guid subIssueId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSubIssueCommand(issueId, subIssueId);
        
        var result = await handler.Handle(
            command,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{subIssueId:guid}/assignee")]
    public async Task<ActionResult<Guid>> SetAssignee(
        [FromRoute] Guid issueId,
        [FromRoute] Guid subIssueId,
        [FromServices] SetSubIssueAssigneeHandler handler,
        [FromBody] SetSubIssueAssigneeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId, subIssueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{subIssueId:guid}/title")]
    public async Task<ActionResult<Guid>> UpdateTitle(
        [FromRoute] Guid issueId,
        [FromRoute] Guid subIssueId,
        [FromServices] UpdateSubIssueTitleHandler handler,
        [FromBody] UpdateSubIssueTitleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId, subIssueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{subIssueId:guid}/description")]
    public async Task<ActionResult<Guid>> UpdateDescription(
        [FromRoute] Guid issueId,
        [FromRoute] Guid subIssueId,
        [FromServices] UpdateSubIssueDescriptionHandler handler,
        [FromBody] UpdateSubIssueDescriptionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId, subIssueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{subIssueId:guid}/status")]
    public async Task<ActionResult<Guid>> UpdateStatus(
        [FromRoute] Guid issueId,
        [FromRoute] Guid subIssueId,
        [FromServices] UpdateSubIssueStatusHandler handler,
        [FromBody] UpdateSubIssueStatusRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId, subIssueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpPut("{subIssueId:guid}/priority")]
    public async Task<ActionResult<Guid>> UpdatePriority(
        [FromRoute] Guid issueId,
        [FromRoute] Guid subIssueId,
        [FromServices] UpdateSubIssuePriorityHandler handler,
        [FromBody] UpdateSubIssuePriorityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            request.ToCommand(issueId, subIssueId),
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));

        return Ok(Envelope.Ok(result.Value));
    }
    
    [HttpGet]
    public async Task<ActionResult<Guid>> GetByIssueId(
        [FromRoute] Guid issueId,
        [FromServices] GetSubIssuesByIssueIdWithPaginationHandler handler,
        [FromQuery] GetSubIssuesByIssueIdWithPaginationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(
            request.ToQuery(issueId),
            cancellationToken);

        return Ok(Envelope.Ok(response));
    }
    
    [HttpGet("{subIssueId:guid}")]
    public async Task<ActionResult<Guid>> GetById(
        [FromServices] GetSubIssuesByIdHandler handler,
        [FromRoute] Guid issueId,
        [FromRoute] Guid subIssueId,
        CancellationToken cancellationToken)
    {
        var query = new GetSubIssuesByIdQuery(issueId, subIssueId);
        
        var result = await handler.Handle(
            query,
            cancellationToken);
        
        if (result.IsFailure)
            return BadRequest(Envelope.Error(result.Error));
        
        return Ok(Envelope.Ok(result.Value));
    }
}