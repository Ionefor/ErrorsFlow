using System.Collections.ObjectModel;

namespace ErrorsFlow.Models;

/// <summary>
/// Неизменяемый непустой список ошибок.
/// Обычно используется для возврата нескольких ошибок валидации.
/// </summary>
public sealed class ErrorList : IReadOnlyList<Error>
{
    private readonly ReadOnlyCollection<Error> _errors;

    /// <summary>
    /// Создаёт список ошибок.
    /// </summary>
    /// <param name="errors">Непустая последовательность ошибок без <see langword="null"/> значений.</param>
    /// <exception cref="ArgumentException">Возникает, если последовательность пуста или содержит <see langword="null"/>.</exception>
    public ErrorList(IEnumerable<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        var items = errors.ToArray();
        if (items.Length == 0)
        {
            throw new ArgumentException("At least one error is required.", nameof(errors));
        }

        if (items.Any(error => error is null))
        {
            throw new ArgumentException("Errors cannot contain null values.", nameof(errors));
        }

        _errors = Array.AsReadOnly(items);
    }

    /// <summary>Количество ошибок в списке.</summary>
    public int Count => _errors.Count;

    /// <summary>Возвращает ошибку по индексу.</summary>
    public Error this[int index] => _errors[index];

    /// <inheritdoc />
    public IEnumerator<Error> GetEnumerator() => _errors.GetEnumerator();

    /// <inheritdoc />
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Создаёт список из одной ошибки.
    /// </summary>
    /// <param name="error">Ошибка для добавления в список.</param>
    /// <returns>Непустой список ошибок.</returns>
    public static ErrorList From(Error error) => new([error]);

    /// <summary>
    /// Неявно преобразует одну ошибку в список из одной ошибки.
    /// </summary>
    /// <param name="error">Ошибка для преобразования.</param>
    public static implicit operator ErrorList(Error error) => From(error);
}
