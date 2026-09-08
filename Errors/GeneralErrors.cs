using ErrorsFlow.Models;

namespace ErrorsFlow.Errors;

/// <summary>
/// Фабрики общих ошибок, не привязанных к предметной области приложения.
/// </summary>
public static class GeneralErrors
{
    /// <summary>Создаёт ошибку известного сбоя операции.</summary>
    /// <param name="message">Необязательное безопасное сообщение.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Failure"/>.</returns>
    public static Error Failed(string? message = null) =>
        Create(GeneralErrorCodes.Failed, message ?? "The operation failed.", ErrorType.Failure);

    /// <summary>Создаёт ошибку непредвиденного сбоя на внешней границе приложения.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.InternalServer"/>.</returns>
    public static Error InternalServer() =>
        Create(GeneralErrorCodes.InternalServer, "An unexpected error occurred.", ErrorType.InternalServer);

    /// <summary>Создаёт ошибку отсутствующего ресурса.</summary>
    /// <param name="resource">Человекочитаемое название ресурса.</param>
    /// <param name="target">Необязательный идентификатор или поле поиска.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.NotFound"/>.</returns>
    public static Error NotFound(string resource, string? target = null) =>
        Create(
            GeneralErrorCodes.NotFound,
            $"{resource} was not found.",
            ErrorType.NotFound,
            target);

    /// <summary>Создаёт ошибку конфликта для уже существующего значения.</summary>
    /// <param name="target">Поле, значение которого уже существует.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Conflict"/>.</returns>
    public static Error ValueAlreadyExists(string target) =>
        Create(
            GeneralErrorCodes.ValueAlreadyExists,
            "The value already exists.",
            ErrorType.Conflict,
            target);

    /// <summary>Создаёт ошибку невалидного значения.</summary>
    /// <param name="target">Поле с невалидным значением.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Validation"/>.</returns>
    public static Error ValueIsInvalid(string target) =>
        Create(
            GeneralErrorCodes.ValueIsInvalid,
            "The value is invalid.",
            ErrorType.Validation,
            target);

    /// <summary>Создаёт ошибку отсутствующего обязательного значения.</summary>
    /// <param name="target">Обязательное поле.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Validation"/>.</returns>
    public static Error ValueIsRequired(string target) =>
        Create(
            GeneralErrorCodes.ValueIsRequired,
            "The value is required.",
            ErrorType.Validation,
            target);

    private static Error Create(string code, string message, ErrorType type, string? target = null) =>
        ErrorFactory.Create(code, message, type, target);
}
