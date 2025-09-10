using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.SubIssue.DeleteSubIssue;

public record DeleteSubIssueCommand(Guid IssueId, Guid SubIssueId) : ICommand;
