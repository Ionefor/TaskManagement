using TaskManagement.Application.Abstractions.CQ;

namespace TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueDescription;

public record UpdateSubIssueDescriptionCommand(
    Guid IssueId,
    Guid SubIssueId,
    string Description) : ICommand;