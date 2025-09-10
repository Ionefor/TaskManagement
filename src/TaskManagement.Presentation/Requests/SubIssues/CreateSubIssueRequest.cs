using TaskManagement.Application.Features.Commands.SubIssue.CreateSubIssue;

namespace TaskManagement.Presentation.Requests.SubIssues;

public record CreateSubIssueRequest(
    string Title,
    string Description,
    string Author,
    string Status,
    string Priority)
{
    public CreateSubIssueCommand ToCommand(Guid issueId) => 
        new(issueId, Title, Description, Author, Status, Priority);
}