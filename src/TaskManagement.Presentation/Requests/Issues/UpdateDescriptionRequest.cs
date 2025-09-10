using TaskManagement.Application.Features.Commands.Issue.UpdateDescription;

namespace TaskManagement.Presentation.Requests.Issues;

public record UpdateDescriptionRequest(string Description)
{
    public UpdateDescriptionCommand ToCommand(Guid issueId) =>
        new(issueId, Description);
}