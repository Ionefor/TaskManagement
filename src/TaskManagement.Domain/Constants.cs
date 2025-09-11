namespace TaskManagement.Domain;

public static class Constants
{
    public static class General
    {
        public const string ConfigurationsWrite = "Configurations.Write";
        public const string ConfigurationsRead = "Configurations.Read";
        public const string Database = "database";
        public const string SchemaName = "TaskManagement";
    }
    
    public static class Validation
    {
        public const int MaxTextLength = 5000;
        public const int MediumTextLength = 100;
        public const int LowTextLength = 20;
    }
    
    public static class Error
    {
        public const string ValueIsRequiredCode = "value.is.required";
        public const string ValueIsInvalidCode = "value.is.invalid";
        public const string NotFoundCode = "value.not.found";
        public const string InternalServerCode = "server.is.internal";
        public const string FailedCode = "failed.operation";
    }
}