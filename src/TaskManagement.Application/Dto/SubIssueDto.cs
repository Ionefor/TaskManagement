using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Dto;

public class SubIssueDto
{
    public Guid Id { get; init; }
    
    public Guid IssueId { get; init; }
    
    public string Title { get; init; }
    
    public string Description { get; init; }
    
    public IssueStatus  Status { get; init; }
    
    public IssuePriority  Priority { get; init; }
    
    public string Author { get; init; }
    
    public string Assignee { get; init; }
}