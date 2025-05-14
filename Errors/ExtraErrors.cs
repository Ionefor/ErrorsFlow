using ErrorsFlow.Models;
using ErrorsFlow.Parameters;

namespace ErrorsFlow.Errors;

public static class ExtraErrors
{
    public static Error InvalidDeleteOperation(ErrorParameters.InvalidDeleteOperation? parameters = null)
    {
        return ErrorFactory.Create(
            ErrorConstants.Code.DeleteIsInvalid,
            ErrorConstants.ExtraMessage.DeleteIsInvalid,
            ErrorType.Conflict,
            parameters is null ? null
                : $"{parameters.SubjectName} with Id: {parameters.SubjectValue} cannot be deleted");
    }
        
    public static Error AlreadyExists(ErrorParameters.ValueAlreadyExists? parameters = null)
    {
        return ErrorFactory.Create(
            ErrorConstants.Code.ValueAlreadyExists,
            ErrorConstants.ExtraMessage.ValueAlreadyExists,
            ErrorType.Conflict,
            parameters?.Message);
    }
}