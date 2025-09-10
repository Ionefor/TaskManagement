using TaskManagement.Domain.Abstractions;

namespace TaskManagement.Domain.ValueObjects;

public class SubIssueId(Guid id) : BaseId<SubIssueId>(id)
{
    public static implicit operator Guid(SubIssueId subIssueId) => subIssueId.Id;
    
    public static implicit operator SubIssueId(Guid id) => new(id);
}