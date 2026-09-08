using System.Collections.ObjectModel;

namespace ErrorsFlow.Models;

/// <summary>
/// Неизменяемое структурированное описание ожидаемой ошибки.
/// Не содержит данных транспорта, исключений и stack trace.
/// </summary>
public sealed record Error
{
    /// <summary>Стабильный машинно-читаемый код ошибки.</summary>
    public string Code { get; }

    /// <summary>Безопасное fallback-сообщение об ошибке.</summary>
    public string Message { get; }

    /// <summary>Общая категория ошибки.</summary>
    public ErrorType Type { get; }

    /// <summary>Поле или ресурс, к которому относится ошибка, если оно известно.</summary>
    public string? Target { get; }

    /// <summary>Неизменяемые дополнительные безопасные данные ошибки.</summary>
    public IReadOnlyDictionary<string, string?> Metadata { get; }

    private Error(
        string code,
        string message,
        ErrorType type,
        string? target,
        IReadOnlyDictionary<string, string?>? metadata)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        Code = code;
        Message = message;
        Type = type;
        Target = target;
        Metadata = CreateMetadata(metadata);
    }

    /// <summary>
    /// Создаёт структурированную ошибку.
    /// </summary>
    /// <param name="code">Стабильный машинно-читаемый код ошибки.</param>
    /// <param name="message">Безопасное fallback-сообщение.</param>
    /// <param name="type">Категория ошибки.</param>
    /// <param name="target">Необязательное поле или ресурс.</param>
    /// <param name="metadata">Необязательные безопасные дополнительные данные.</param>
    /// <returns>Новая неизменяемая ошибка.</returns>
    public static Error Create(
        string code,
        string message,
        ErrorType type,
        string? target = null,
        IReadOnlyDictionary<string, string?>? metadata = null) =>
        new(code, message, type, target, metadata);

    /// <summary>
    /// Преобразует одну ошибку в непустой список ошибок.
    /// </summary>
    /// <returns>Список, содержащий текущую ошибку.</returns>
    public ErrorList ToErrorList() => ErrorList.From(this);

    private static IReadOnlyDictionary<string, string?> CreateMetadata(
        IReadOnlyDictionary<string, string?>? metadata)
    {
        var values = metadata is null
            ? new Dictionary<string, string?>(StringComparer.Ordinal)
            : new Dictionary<string, string?>(metadata, StringComparer.Ordinal);

        return new ReadOnlyDictionary<string, string?>(values);
    }
}
