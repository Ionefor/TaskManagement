using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.SubIssue.CreateSubIssue;

public record CreateSubIssueCommand(
    Guid IssueId,
    string Title,
    string Description,
    string Author,
    string Status,
    string Priority) : ICommand;