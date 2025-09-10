using TaskManagement.Application.Abstractions;
using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Issue.UpdateStatus;

public record UpdateStatusCommand(Guid IssueId, string Status) : ICommand;