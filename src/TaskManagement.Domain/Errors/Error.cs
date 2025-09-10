using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Errors;

public record Error
{
    public const string Separator = "||";

    private Error(
        string code,
        string message,
        ErrorType type,
        string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }

    public string? InvalidField { get; }

    internal static Error Validation(string code, string message, string? invalidField = null)
    {
        return new Error(code, message, ErrorType.Validation, invalidField);
    }

    internal static Error NotFound(string code, string message)
    {
        return new Error(code, message, ErrorType.NotFound);
    }

    internal static Error Failure(string code, string message)
    {
        return new Error(code, message, ErrorType.Failure);
    }

    internal static Error Conflict(string code, string message)
    {
        return new Error(code, message, ErrorType.Conflict);
    }

    public string Serialize()
    {
        return string.Join(Separator, Code, Message, Type);
    }

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(Separator);

        if (parts.Length < 3) throw new ArgumentException("Invalid serialized format");

        if (!Enum.TryParse<ErrorType>(parts[2], out var type))
            throw new ArgumentException("Invalid serialized format");

        return new Error(parts[0], parts[1], type);
    }

    public ErrorList ToErrorList()
    {
        return new ErrorList([this]);
    }
}