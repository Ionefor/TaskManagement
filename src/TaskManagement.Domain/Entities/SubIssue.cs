using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Entities;

public class SubIssue : BaseEntity<SubIssueId>
{
    private SubIssue(SubIssueId id) : base(id) {}
    
    public SubIssue(
        SubIssueId id,
        Title title,
        Description description,
        IssueStatus  status,
        IssuePriority priority,
        Name author) : base(id)
    {
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
        Author = author;
    }
    
    public Title Title { get; private set; }
    
    public Description Description { get; private set; }
    
    public IssueStatus  Status { get; private set; }
    
    public IssuePriority  Priority { get; private set; }
    
    public Name Author { get; init; }
    
    public Name Assignee { get; private set; }
    
    internal void SetAssignee(Name assignee)
    {
        Assignee = assignee;
        UpdatedAt = DateTime.UtcNow;
    }
    
    internal void UpdateTitle(Title title)
    {
        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void UpdateDescription(Description description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void UpdateStatus(IssueStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void UpdatePriority(IssuePriority priority)
    {
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }
}