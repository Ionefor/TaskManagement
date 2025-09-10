using System.ComponentModel;
using CSharpFunctionalExtensions;
using TaskManagement.Domain.Errors;

namespace TaskManagement.Domain.ValueObjects;

public class Title : ComparableValueObject
{
    private Title() {}

    private Title(string value)
    {
        Value = value;
    }
    
    public string Value { get; }
    
    public static Result<Title, Error> Create(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length > 20)
        {
            return Errors.Errors.General.
                ValueIsInvalid("Title is invalid");
        }
        
        return new Title(value);
    }
    
    protected override IEnumerable<IComparable> GetComparableEqualityComponents()
    {
        yield return Value;
    }
}