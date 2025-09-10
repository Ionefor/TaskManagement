using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Issue.SetAssignee;

public record SetAssigneeCommand(Guid IssueId, string Assignee) : ICommand;
