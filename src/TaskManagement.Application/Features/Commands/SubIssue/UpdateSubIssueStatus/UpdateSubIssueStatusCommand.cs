using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueStatus;

public record UpdateSubIssueStatusCommand(
    Guid IssueId,
    Guid SubIssueId,
    string Status) : ICommand;