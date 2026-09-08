namespace ErrorsFlow.Errors;

/// <summary>
/// Стабильные коды ошибок аутентификации и авторизации.
/// </summary>
public static class AuthErrorCodes
{
    /// <summary>Аутентифицированному пользователю запрещён доступ к операции.</summary>
    public const string AccessForbidden = "auth.access.forbidden";

    /// <summary>Учётные данные недействительны.</summary>
    public const string CredentialsInvalid = "auth.credentials.invalid";

    /// <summary>Refresh token истёк.</summary>
    public const string RefreshTokenExpired = "auth.refresh.token.expired";

    /// <summary>Refresh token недействителен.</summary>
    public const string RefreshTokenInvalid = "auth.refresh.token.invalid";

    /// <summary>Роль не соответствует требуемой роли операции.</summary>
    public const string RoleInvalid = "auth.role.invalid";

    /// <summary>Access token истёк.</summary>
    public const string TokenExpired = "auth.token.expired";

    /// <summary>Access token недействителен.</summary>
    public const string TokenInvalid = "auth.token.invalid";

    /// <summary>Для операции требуется аутентификация.</summary>
    public const string Unauthorized = "auth.unauthorized";
}
