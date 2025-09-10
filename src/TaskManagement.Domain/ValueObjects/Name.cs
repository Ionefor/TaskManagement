using CSharpFunctionalExtensions;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Domain.ValueObjects;

public class Name : ComparableValueObject
{
    private Name() {}

    private Name(string value)
    {
        Value = value;
    }
    public string Value { get; }
    
    public static Result<Name, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 20)
        {
            return Errors.Errors.General.
                ValueIsInvalid("Name is invalid");
        }
        
        return new Name(value);
    }
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}