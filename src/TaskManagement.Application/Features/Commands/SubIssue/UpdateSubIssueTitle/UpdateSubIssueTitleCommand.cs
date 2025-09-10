using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueTitle;

public record UpdateSubIssueTitleCommand(
    Guid IssueId,
    Guid SubIssueId,
    string Title) : ICommand;