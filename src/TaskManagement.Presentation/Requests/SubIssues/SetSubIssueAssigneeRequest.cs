using TaskManagement.Application.Features.Commands.SubIssue.SetSubIssueAssignee;

namespace TaskManagement.Presentation.Requests.SubIssues;

public record SetSubIssueAssigneeRequest(string Assignee)
{
    public SetSubIssueAssigneeCommand ToCommand(
        Guid issueId,
        Guid subIssueId) => 
        new(issueId, subIssueId, Assignee);
}