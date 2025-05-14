namespace ErrorsFlow.Parameters;

public static partial class ErrorParameters
{
    public record InvalidDeleteOperation(string SubjectName, object SubjectValue);
    public record ValueAlreadyExists(string Message);
}