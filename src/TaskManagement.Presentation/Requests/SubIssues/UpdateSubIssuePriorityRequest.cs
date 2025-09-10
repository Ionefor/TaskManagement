using TaskManagement.Application.Features.Commands.SubIssue.UpdateSubIssuePriority;

namespace TaskManagement.Presentation.Requests.SubIssues;

public record UpdateSubIssuePriorityRequest(string Priority)
{
    public UpdateSubIssuePriorityCommand ToCommand(
        Guid issueId,
        Guid subIssueId) => 
        new(issueId, subIssueId, Priority);
}