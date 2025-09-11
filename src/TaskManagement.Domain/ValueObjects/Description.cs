using CSharpFunctionalExtensions;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Domain.ValueObjects;

public class Description : ComparableValueObject
{
    private Description() {}

    private Description(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value) ||
            value.Length > Constants.Validation.MaxTextLength)
        {
            return Errors.Errors.General.
                ValueIsInvalid(nameof(Description));
        }
        
        return new Description(value);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}