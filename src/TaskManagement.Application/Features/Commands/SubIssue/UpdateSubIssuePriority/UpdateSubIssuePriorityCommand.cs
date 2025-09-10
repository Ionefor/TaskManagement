using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssuePriority;

public record UpdateSubIssuePriorityCommand(
    Guid IssueId,
    Guid SubIssueId,
    string Priority) : ICommand;