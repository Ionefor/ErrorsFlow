using ErrorsFlow.Models;

namespace ErrorsFlow.Errors;

/// <summary>
/// Фабрики типовых ошибок аутентификации и авторизации.
/// </summary>
public static class AuthErrors
{
    /// <summary>Создаёт ошибку запрета доступа для аутентифицированного пользователя.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.Forbidden"/>.</returns>
    public static Error AccessForbidden() =>
        Create(AuthErrorCodes.AccessForbidden, "Access is forbidden.", ErrorType.Forbidden);

    /// <summary>Создаёт ошибку недействительных учётных данных.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.Unauthorized"/>.</returns>
    public static Error CredentialsInvalid() =>
        Create(AuthErrorCodes.CredentialsInvalid, "The credentials are invalid.", ErrorType.Unauthorized);

    /// <summary>Создаёт ошибку истёкшего refresh token.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.Unauthorized"/>.</returns>
    public static Error RefreshTokenExpired() =>
        Create(AuthErrorCodes.RefreshTokenExpired, "The refresh token has expired.", ErrorType.Unauthorized);

    /// <summary>Создаёт ошибку недействительного refresh token.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.Unauthorized"/>.</returns>
    public static Error RefreshTokenInvalid() =>
        Create(AuthErrorCodes.RefreshTokenInvalid, "The refresh token is invalid.", ErrorType.Unauthorized);

    /// <summary>Создаёт ошибку роли, не соответствующей требуемой роли операции.</summary>
    /// <param name="expectedRole">Необязательное название ожидаемой роли.</param>
    /// <returns>Ошибка с типом <see cref="ErrorType.Validation"/> и target <c>role</c>.</returns>
    public static Error RoleIsInvalid(string? expectedRole = null) =>
        ErrorFactory.Create(
            AuthErrorCodes.RoleInvalid,
            "The role is invalid.",
            ErrorType.Validation,
            "role",
            expectedRole is null ? null : new Dictionary<string, string?>
            {
                ["expectedRole"] = expectedRole
            });

    /// <summary>Создаёт ошибку истёкшего access token.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.Unauthorized"/>.</returns>
    public static Error TokenExpired() =>
        Create(AuthErrorCodes.TokenExpired, "The token has expired.", ErrorType.Unauthorized);

    /// <summary>Создаёт ошибку недействительного access token.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.Unauthorized"/>.</returns>
    public static Error TokenInvalid() =>
        Create(AuthErrorCodes.TokenInvalid, "The token is invalid.", ErrorType.Unauthorized);

    /// <summary>Создаёт ошибку отсутствующей аутентификации.</summary>
    /// <returns>Ошибка с типом <see cref="ErrorType.Unauthorized"/>.</returns>
    public static Error Unauthorized() =>
        Create(AuthErrorCodes.Unauthorized, "Authentication is required.", ErrorType.Unauthorized);

    private static Error Create(string code, string message, ErrorType type) =>
        ErrorFactory.Create(code, message, type);
}
