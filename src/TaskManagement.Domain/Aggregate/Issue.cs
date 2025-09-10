using CSharpFunctionalExtensions;
using TaskManagement.Domain.Abstractions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Errors;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Domain.Aggregate;

public class Issue : BaseEntity<IssueId>
{
    private readonly List<SubIssue> _subIssues = [];
    
    private Issue(IssueId id) : base(id) {}

    public Issue(
        IssueId id,
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
    
    public IssueStatus Status { get; private set; }
    
    public IssuePriority Priority { get; private set; }
    
    public Name Author { get; init; }
    
    public Name Assignee { get; private set; }
    
    public IReadOnlyList<SubIssue> SubIssues => _subIssues;
    
    public void SetAssignee(Name assignee)
    {
        Assignee = assignee;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void UpdateTitle(Title title)
    {
        Title = title;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDescription(Description description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(IssueStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdatePriority(IssuePriority priority)
    {
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddSubIssue(SubIssue subIssue)
    {
        _subIssues.Add(subIssue);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveSubIssue(SubIssue subIssue)
    {
        _subIssues.Remove(subIssue);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public UnitResult<Error> SetSubIssueAssignee(
        SubIssueId subIssueId, Name assignee)
    {
        var subIssue = _subIssues.
            SingleOrDefault(s => s.Id == subIssueId);

        if (subIssue is null)
        {
            return Errors.Errors.
                General.NotFound("SubTask not found");
        }
        
        subIssue.SetAssignee(assignee);
        
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> UpdateSubIssueTitle(
        SubIssueId subIssueId, Title title)
    {
        var subIssue = _subIssues.
            SingleOrDefault(s => s.Id == subIssueId);

        if (subIssue is null)
        {
            return Errors.Errors.
                General.NotFound("SubTask not found");
        }
        
        subIssue.UpdateTitle(title);
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdateSubIssueDescription(
        SubIssueId subIssueId, Description description)
    {
        var subIssue = _subIssues.
            SingleOrDefault(s => s.Id == subIssueId);

        if (subIssue is null)
        {
            return Errors.Errors.
                General.NotFound("SubIssue not found");
        }
        
        subIssue.UpdateDescription(description);
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdateSubIssueStatus(
        SubIssueId subIssueId, IssueStatus status)
    {
        var subIssue = _subIssues.
            SingleOrDefault(s => s.Id == subIssueId);

        if (subIssue is null)
        {
            return Errors.Errors.
                General.NotFound("SubIssue not found");
        }
        
        subIssue.UpdateStatus(status);
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdateSubIssuePriority(
        SubIssueId subIssueId, IssuePriority priority)
    {
        var subIssue = _subIssues.
            SingleOrDefault(s => s.Id == subIssueId);

        if (subIssue is null)
        {
            return Errors.Errors.
                General.NotFound("SubIssue not found");
        }
        
        subIssue.UpdatePriority(priority);
        
        return Result.Success<Error>();
    }
}