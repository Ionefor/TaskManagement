using TaskManagement.Application.Features.Commands.Issue.UpdateStatus;

namespace TaskManagement.Presentation.Requests.Issues;

public record UpdateStatusRequest(string Status)
{
    public UpdateStatusCommand ToCommand(Guid issueId) =>
        new (issueId, Status);
}