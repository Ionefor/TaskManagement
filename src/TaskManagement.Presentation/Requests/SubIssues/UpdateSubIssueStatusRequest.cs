using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssueStatus;

namespace TaskManagement.Presentation.Requests.SubIssues;

public record UpdateSubIssueStatusRequest(string Status)
{
    public UpdateSubIssueStatusCommand ToCommand(
        Guid issueId,
        Guid subIssueId) => 
        new(issueId, subIssueId, Status);
}