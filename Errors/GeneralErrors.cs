using ErrorsFlow.Models;
using ErrorsFlow.Parameters;

namespace ErrorsFlow.Errors;

public static class GeneralErrors
{
        public static Error ValueIsInvalid(ErrorParameters.ValueIsInvalid? parameters = null)
        {
            return ErrorFactory.Create(
                ErrorConstants.Code.ValueIsInvalid,
                ErrorConstants.GeneralMessage.ValueIsInvalid,
                ErrorType.Validation,
                parameters?.SubjectName is null ? null : $"{parameters.SubjectName} is invalid",
                parameters?.SubjectName);
        }

        public static Error NotFound(ErrorParameters.NotFound? parameters = null)
        {
            return ErrorFactory.Create(
                ErrorConstants.Code.NotFound,
                ErrorConstants.GeneralMessage.NotFound,
                ErrorType.NotFound,
                parameters is null
                    ? null
                    : $"{parameters.ObjectName} record not found with {parameters.SubjectType}:" +
                      $" {parameters.SubjectValue}");
        }

        public static Error ValueIsRequired(ErrorParameters.ValueIsRequired? parameters = null)
        {
            return ErrorFactory.Create(
                ErrorConstants.Code.ValueIsRequired,
                ErrorConstants.GeneralMessage.ValueIsRequired,
                ErrorType.Validation,
                parameters is null
                    ? null
                    : $"{parameters.SubjectName} cannot be null or empty");
        }

        public static Error InternalServer(ErrorParameters.InternalServer? parameters = null)
        {
            return ErrorFactory.Create(
                ErrorConstants.Code.InternalServer,
                ErrorConstants.GeneralMessage.InternalServer,
                ErrorType.InternalServer,
                parameters?.Message);
        }

        public static Error Failed(ErrorParameters.Failed? parameters = null)
        {
            return ErrorFactory.Create(
                ErrorConstants.Code.Failed,
                ErrorConstants.GeneralMessage.Failed,
                ErrorType.Failure,
                parameters?.Message);
        }
    }