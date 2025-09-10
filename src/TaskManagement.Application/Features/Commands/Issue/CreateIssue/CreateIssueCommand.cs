using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.Issue.CreateIssue;

public record CreateIssueCommand(
    string Title,
    string Description,
    string Author,
    string Status,
    string Priority) : ICommand;