namespace ErrorsFlow.Errors;

/// <summary>
/// Стабильные коды общих ошибок, не зависящих от предметной области.
/// </summary>
public static class GeneralErrorCodes
{
    /// <summary>Операция не выполнена по известной причине.</summary>
    public const string Failed = "operation.failed";

    /// <summary>На внешней границе приложения возникла непредвиденная ошибка.</summary>
    public const string InternalServer = "server.internal";

    /// <summary>Запрошенный ресурс не найден.</summary>
    public const string NotFound = "resource.not.found";

    /// <summary>Значение уже существует и конфликтует с операцией.</summary>
    public const string ValueAlreadyExists = "value.already.exists";

    /// <summary>Переданное значение не соответствует правилу валидации.</summary>
    public const string ValueIsInvalid = "value.is.invalid";

    /// <summary>Обязательное значение не передано.</summary>
    public const string ValueIsRequired = "value.is.required";
}
