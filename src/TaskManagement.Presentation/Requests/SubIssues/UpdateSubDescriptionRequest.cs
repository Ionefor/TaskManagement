using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueDescription;

namespace TaskManagement.Presentation.Requests.SubIssues;

public record UpdateSubIssueDescriptionRequest(string Description)
{
    public UpdateSubIssueDescriptionCommand ToCommand(
        Guid issueId,
        Guid subIssueId) =>
        new(issueId, subIssueId, Description);
}