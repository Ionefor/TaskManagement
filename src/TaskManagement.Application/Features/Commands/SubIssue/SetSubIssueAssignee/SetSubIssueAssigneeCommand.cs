using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.SubIssue.SetSubIssueAssignee;

public record SetSubIssueAssigneeCommand(
    Guid IssueId,
    Guid SubIssueId,
    string Assignee) : ICommand;