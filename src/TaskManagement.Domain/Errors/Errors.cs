namespace TaskManagement.Domain.Errors;

public static class Errors
{
    public static class General
    {
        public static Error ValueIsInvalid(string propertyName)
        {
            return Error.Validation(
                Constants.Error.ValueIsInvalidCode,
                $"{propertyName} is not a valid value.");
        }

        public static Error NotFound(string subjectName)
        {
            return Error.NotFound(
                Constants.Error.NotFoundCode,
                $"record {subjectName} not found.");
        }

        public static Error ValueIsRequired(string propertyName)
        {
            return Error.Validation(
                Constants.Error.ValueIsRequiredCode,
                $"{propertyName} is required.");
        }

        public static Error InternalServer(string message)
        {
            return Error.Failure(
                Constants.Error.InternalServerCode,
                message);
        }

        public static Error Failed(string message)
        {
            return Error.Failure(
                Constants.Error.FailedCode,
                message);
        }
    }
}