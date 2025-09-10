using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Issue.UpdateTitle;

public record UpdateTitleCommand(Guid IssueId, string Title) : ICommand;