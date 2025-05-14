using ErrorsFlow.Models;
using ErrorsFlow.Parameters;

namespace ErrorsFlow.Errors;

public static class AuthErrors
{
    public static Error TokenIsExpired()
    {
        return ErrorFactory.Create(
            ErrorConstants.Code.ValueIsInvalid,
            ErrorConstants.AuthMessage.ExpiredToken,
            ErrorType.Failure);
    }
    
    public static Error TokenIsInvalid()
    {
        return ErrorFactory.Create(
            ErrorConstants.Code.ValueIsInvalid,
            ErrorConstants.AuthMessage.InvalidToken,
            ErrorType.Validation);
    }
    
    public static Error RoleIsInvalid(ErrorParameters.RoleIsInvalid? parameters = null)
    {
        return ErrorFactory.Create(
            ErrorConstants.Code.ValueIsInvalid,
            ErrorConstants.AuthMessage.InvalidRole,
            ErrorType.Conflict,
            parameters?.Message);
    }
    
    public static Error CredentialsIsInvalid()
    {
        return ErrorFactory.Create(
            ErrorConstants.Code.ValueIsInvalid,
            ErrorConstants.AuthMessage.InvalidCredentials,
            ErrorType.Validation);
    }
}