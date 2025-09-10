using TaskManagement.Domain.Abstractions;

namespace TaskManagement.Domain.ValueObjects;

public class IssueId(Guid id) : BaseId<IssueId>(id)
{
    public static implicit operator Guid(IssueId issueId) => issueId.Id;
    
    public static implicit operator IssueId(Guid id) => new(id);
}