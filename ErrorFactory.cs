using ErrorsFlow.Models;

namespace ErrorsFlow;

/// <summary>
/// Создаёт экземпляры <see cref="Error"/> в едином формате.
/// Используется общими и прикладными каталогами ошибок.
/// </summary>
public static class ErrorFactory
{
    /// <summary>
    /// Создаёт структурированную ошибку.
    /// </summary>
    /// <param name="code">Стабильный машинно-читаемый код ошибки.</param>
    /// <param name="message">Безопасное fallback-сообщение.</param>
    /// <param name="type">Категория ошибки.</param>
    /// <param name="target">Необязательное поле или ресурс, к которому относится ошибка.</param>
    /// <param name="metadata">Необязательные безопасные дополнительные данные.</param>
    /// <returns>Новая неизменяемая ошибка.</returns>
    public static Error Create(
        string code,
        string message,
        ErrorType type,
        string? target = null,
        IReadOnlyDictionary<string, string?>? metadata = null) =>
        Error.Create(code, message, type, target, metadata);
}
