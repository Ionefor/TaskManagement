using CSharpFunctionalExtensions;

namespace TaskManagement.Domain.Abstractions;

public abstract class BaseEntity<TId> : Entity<TId> where TId : BaseId<TId>
{
    protected BaseEntity(TId id) : base(id)
    {
        CreatedAt = DateTime.UtcNow;
    }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; protected set; }
}