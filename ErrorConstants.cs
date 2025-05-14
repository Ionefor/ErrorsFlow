namespace ErrorsFlow;

public static class ErrorConstants
{
    public static class Code
    {
        public const string InternalServer = "server.is.internal";
        public const string Failed = "failed.operation";
        public const string NotFound = "value.not.found";
        public const string ValueIsRequired = "value.is.required";
        public const string ValueIsInvalid = "value.is.invalid";
        
        public const string DeleteIsInvalid = "cannot.delete.operation";
        public const string ValueAlreadyExists = "value.already.exists";
    }
   
    public static class GeneralMessage
    {
        public const string InternalServer = "Server is internal";
        public const string Failed = "Failed to do operation";
        public const string NotFound = "Value not found";
        public const string ValueIsRequired = "Value cannot be null or empty";
        public const string ValueIsInvalid = "Value is invalid";
    }
    
    public static class ExtraMessage
    {
        public const string ValueAlreadyExists = "Value already exists";
        public const string DeleteIsInvalid = "Cannot be deleted";
    }
    
    public static class AuthMessage
    {
        public const string InvalidToken = "Token is invalid";
        public const string ExpiredToken = "Token is expired";
        public const string InvalidRole = "Role is invalid";
        public const string InvalidCredentials = "Your credentials is invalid";
    }
}