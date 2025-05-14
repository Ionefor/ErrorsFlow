namespace ErrorsFlow.Parameters;

public static partial class ErrorParameters
{
    public record NotFound(string ObjectName, string SubjectType, object SubjectValue);
    public record ValueIsRequired(string SubjectName);
    public record ValueIsInvalid(string SubjectName);
    public record InternalServer(string Message);
    public record Failed(string Message);
}