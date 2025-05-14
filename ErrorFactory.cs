using ErrorsFlow.Models;

namespace ErrorsFlow;

public static class ErrorFactory
{
    public static Error Create(
        string code,
        string defaultMessage,
        ErrorType type,
        string? customMessage = null,
        string? invalidField = null)
    {
        return Error.CreateError(
            code,
            customMessage ?? defaultMessage,
            type,
            invalidField);
    }
}