using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Issue.DeleteIssue;

public record DeleteIssueCommand(Guid IssueId) : ICommand;