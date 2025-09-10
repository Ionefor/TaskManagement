using TaskManagement.Application.Features.Commands.Issue.UpdateTitle;

namespace TaskManagement.Presentation.Requests.Issues;

public record UpdateTitleRequest(string Title)
{
    public UpdateTitleCommand ToCommand(Guid issueId) => 
        new(issueId, Title);
}