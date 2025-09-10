using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Issue.UpdateDescription;

public record UpdateDescriptionCommand(Guid IssueId, string Description) : ICommand;