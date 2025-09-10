using TaskManagement.Application.Features.Commands.Issue.SetAssignee;

namespace TaskManagement.Presentation.Requests.Issues;

public record SetAssigneeRequest(string Assignee)
{
    public SetAssigneeCommand ToCommand(Guid issueId) => 
        new(issueId, Assignee);
}