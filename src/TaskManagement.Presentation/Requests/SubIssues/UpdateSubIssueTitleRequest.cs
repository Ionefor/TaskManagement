using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueTitle;

namespace TaskManagement.Presentation.Requests.SubIssues;

public record UpdateSubIssueTitleRequest(string Title)
{
    public UpdateSubIssueTitleCommand ToCommand(
        Guid issueId,
        Guid subIssueId) =>
        new(issueId, subIssueId, Title);
}