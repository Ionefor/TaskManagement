using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Issue.UpdatePriority;

public record UpdatePriorityCommand(Guid IssueId, string Priority) : ICommand;