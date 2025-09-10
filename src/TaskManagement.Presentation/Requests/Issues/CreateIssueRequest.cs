using TaskManagement.Application.Features.Commands.Issue.CreateIssue;

namespace TaskManagement.Presentation.Requests.Issues;

public record CreateIssueRequest(
    string Title,
    string Description,
    string Author,
    string Status,
    string Priority)
{
    public CreateIssueCommand ToCommand() => 
        new(Title, Description, Author, Status, Priority);
}