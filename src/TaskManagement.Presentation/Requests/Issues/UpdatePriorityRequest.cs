using TaskManagement.Application.Features.Commands.Issue.UpdatePriority;

namespace TaskManagement.Presentation.Requests.Issues;

public record UpdatePriorityRequest(string Priority)
{
    public UpdatePriorityCommand ToCommand(Guid issueId) => 
        new(issueId, Priority);
}