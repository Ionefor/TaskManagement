namespace TaskManagement.Domain.Errors;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string message)
        {
            return Error.Validation("value.is.invalid", message);
        }

        public static Error NotFound(string message)
        {
            return Error.NotFound("value.not.found", message);
        }

        public static Error ValueIsRequired(string message)
        {
            return Error.Validation("value.is.required", message);
        }

        public static Error InternalServer(string message)
        {
            return Error.Failure("server.is.internal", message);
        }

        public static Error Failed(string message)
        {
            return Error.Failure("failed.operation", message);
        }
    }
}