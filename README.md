# ErrorsFlow

`ErrorsFlow` — лёгкая независимая от транспорта библиотека для представления ожидаемых ошибок в .NET-приложениях.

Она предоставляет единый формат ошибки, общие каталоги ошибок для базовых сценариев и аутентификации, а также соглашения для ошибок конкретного приложения. Библиотека намеренно не зависит от ASP.NET Core, EF Core, брокеров сообщений и предметной области.

> Версия 2.0.0 содержит несовместимые изменения и пока не опубликована.

## Содержание

- [Установка](#установка)
- [Модель ошибки](#модель-ошибки)
- [Встроенные ошибки](#встроенные-ошибки)
- [Ошибки конкретного приложения](#ошибки-конкретного-приложения)
- [Использование с CSharpFunctionalExtensions](#использование-с-csharpfunctionalextensions)
- [Несколько ошибок](#несколько-ошибок)
- [Рекомендации для HTTP API](#рекомендации-для-http-api)
- [Соглашения для кодов и metadata](#соглашения-для-кодов-и-metadata)
- [Состав библиотеки](#состав-библиотеки)
- [Миграция с 1.x](#миграция-с-1x)
- [Разработка](#разработка)

## Установка

После публикации пакета:

```bash
dotnet add package ErrorsFlow
```

`ErrorsFlow` содержит только модель ошибок. Для result pattern подключите `CSharpFunctionalExtensions` в проекте-потребителе:

```bash
dotnet add package CSharpFunctionalExtensions
```

## Модель ошибки

`Error` неизменяем и содержит только безопасные структурированные данные:

```csharp
public sealed record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public string? Target { get; }
    public IReadOnlyDictionary<string, string?> Metadata { get; }
}
```

| Свойство | Назначение |
|---|---|
| `Code` | Стабильный машинно-читаемый идентификатор. Его используют клиенты, логи и мониторинг. |
| `Message` | Безопасное fallback-сообщение для разработчика или пользователя. Не помещайте сюда секреты и внутреннюю диагностику. |
| `Type` | Общая категория ошибки. Обычно используется на границе приложения, например для выбора HTTP-статуса. |
| `Target` | Необязательное поле или ресурс, к которому относится ошибка: `email`, `password`, `startAt`. |
| `Metadata` | Необязательные неизменяемые данные «ключ–значение», которые помогают вызывающему коду обработать ошибку. |

Если готовый метод каталога не подходит, ошибку можно создать напрямую:

```csharp
using ErrorsFlow;
using ErrorsFlow.Models;

var error = ErrorFactory.Create(
    code: "users.email.already.exists",
    message: "A user with this email already exists.",
    type: ErrorType.Conflict,
    target: "email",
    metadata: new Dictionary<string, string?>
    {
        ["email"] = "client@example.com"
    });
```

### Категории ошибок

```csharp
public enum ErrorType
{
    Validation,
    NotFound,
    Failure,
    Conflict,
    InternalServer,
    Unauthorized,
    Forbidden
}
```

`Failure` означает известную ошибку выполнения, для которой нет более точной категории. `InternalServer` следует создавать только на внешней границе приложения при неожиданном исключении; доменный код должен возвращать конкретные бизнес-ошибки.

## Встроенные ошибки

Библиотека предоставляет два переиспользуемых каталога:

```csharp
using ErrorsFlow.Errors;
```

### GeneralErrors

```csharp
GeneralErrors.ValueIsRequired("email");
GeneralErrors.ValueIsInvalid("password");
GeneralErrors.ValueAlreadyExists("email");
GeneralErrors.NotFound("User", "userId");
GeneralErrors.Failed();
GeneralErrors.InternalServer();
```

| Метод | Код ошибки | Тип |
|---|---|---|
| `ValueIsRequired` | `value.is.required` | `Validation` |
| `ValueIsInvalid` | `value.is.invalid` | `Validation` |
| `ValueAlreadyExists` | `value.already.exists` | `Conflict` |
| `NotFound` | `resource.not.found` | `NotFound` |
| `Failed` | `operation.failed` | `Failure` |
| `InternalServer` | `server.internal` | `InternalServer` |

### AuthErrors

```csharp
AuthErrors.CredentialsInvalid();
AuthErrors.TokenInvalid();
AuthErrors.TokenExpired();
AuthErrors.RefreshTokenInvalid();
AuthErrors.RefreshTokenExpired();
AuthErrors.RoleIsInvalid("Master");
AuthErrors.Unauthorized();
AuthErrors.AccessForbidden();
```

Ошибки аутентификации используют пространство кодов `auth.*`, например `auth.credentials.invalid`, `auth.token.expired` и `auth.refresh.token.expired`.

## Ошибки конкретного приложения

`ErrorsFlow` не должен превращаться в каталог ошибок всех предметных областей. Бизнес-ошибки хранятся в модуле, которому принадлежит соответствующее бизнес-правило.

Например, модуль `Users` может определить собственные коды и каталог:

```csharp
using ErrorsFlow;
using ErrorsFlow.Models;

public static class UsersErrorCodes
{
    public const string EmailAlreadyExists = "users.email.already.exists";
    public const string MasterRoleRequired = "users.master.role.required";
}

public static class UsersErrors
{
    public static Error EmailAlreadyExists() =>
        ErrorFactory.Create(
            UsersErrorCodes.EmailAlreadyExists,
            "A user with this email already exists.",
            ErrorType.Conflict,
            "email");

    public static Error MasterRoleRequired() =>
        ErrorFactory.Create(
            UsersErrorCodes.MasterRoleRequired,
            "The Master role is required.",
            ErrorType.Validation,
            "role");
}
```

Статические каталоги ошибок не нужно наследовать от `GeneralErrors` или `AuthErrors`. Они используют общие `Error`, `ErrorType` и `ErrorFactory` напрямую.

## Использование с CSharpFunctionalExtensions

Используйте `CSharpFunctionalExtensions` для `Result`, а `ErrorsFlow.Models.Error` — как тип ошибки:

```csharp
using CSharpFunctionalExtensions;
using ErrorsFlow.Errors;
using ErrorsFlow.Models;

public static Result<User, Error> Register(string email)
{
    if (string.IsNullOrWhiteSpace(email))
    {
        return GeneralErrors.ValueIsRequired("email");
    }

    return new User(email);
}
```

Ответственность при этом разделена:

- `ErrorsFlow` определяет структуру ошибки;
- `CSharpFunctionalExtensions` управляет потоком успеха и неуспеха;
- приложение определяет бизнес-ошибки и use case.

Используйте `Result` для ожидаемых бизнес-сценариев. Исключения оставьте для непредвиденных технических сбоев, программных ошибок и нарушений контрактов, которые вызывающий код не может обработать.

## Несколько ошибок

Используйте `ErrorList`, когда валидация должна вернуть несколько проблем:

```csharp
using ErrorsFlow.Errors;
using ErrorsFlow.Models;

var errors = new ErrorList([
    GeneralErrors.ValueIsRequired("email"),
    GeneralErrors.ValueIsInvalid("password")
]);
```

`ErrorList` неизменяем и не может быть пустым. Один `Error` можно неявно преобразовать в `ErrorList`:

```csharp
ErrorList errors = GeneralErrors.ValueIsRequired("email");
```

## Рекомендации для HTTP API

`ErrorsFlow` не ссылается на ASP.NET Core и не формирует HTTP-ответы. Ошибки следует преобразовывать в `ProblemDetails` на границе API.

Рекомендуемое сопоставление:

| Тип ошибки | Типичный HTTP-статус |
|---|---:|
| `Validation` | 400 Bad Request |
| `Unauthorized` | 401 Unauthorized |
| `Forbidden` | 403 Forbidden |
| `NotFound` | 404 Not Found |
| `Conflict` | 409 Conflict |
| `Failure` | Зависит от use case |
| `InternalServer` | 500 Internal Server Error |

Не возвращайте в `Message` или `Metadata` текст исключений, stack trace, ошибки базы данных, токены, пароли и другие чувствительные данные.

## Соглашения для кодов и metadata

Используйте коды в нижнем регистре, разделяя смысловые сегменты точками:

```text
auth.credentials.invalid
users.email.already.exists
profiles.master.not.found
bookings.slot.not.available
```

Рекомендуемая форма кода:

```text
<module>.<subject>.<reason>
```

После релиза считайте `Code` публичным контрактом: его изменение может сломать мобильные клиенты, API-потребителей, правила алертинга и аналитику.

Metadata должна быть небольшой и безопасной. Подходящие данные: идентификаторы, ожидаемые значения, ограничения и имена полей. Не помещайте в неё пароли, access/refresh token, персональные документы, stack trace, connection string и SQL-текст.

## Состав библиотеки

| Находится в ErrorsFlow | Находится в приложении или модуле |
|---|---|
| `Error`, `ErrorList`, `ErrorType`, `ErrorFactory` | `UsersErrors`, `BookingErrors`, `ProfileErrors` |
| Общие каталоги ошибок и ошибок аутентификации | Коды и сообщения предметной области |
| Общие соглашения для кодов | HTTP-маппинг и локализация |
| Нет зависимостей от фреймворков | EF Core, ASP.NET Core, RabbitMQ, logging adapters |

## Миграция с 1.x

Версия 2.0.0 содержит несовместимые изменения API:

- `Error` стал sealed неизменяемым record;
- `InvalidField` переименован в `Target`;
- удалены `Timestamp` и `StackTrace`;
- удалены `Serialize()` и `Deserialize()`; если на транспортной границе нужна сериализация, используйте JSON;
- удалены типы `ErrorParameters`; методы каталогов принимают явные аргументы;
- коды ошибок используют сегменты, разделённые точками, например `resource.not.found`;
- в `ErrorType` добавлены `Unauthorized` и `Forbidden`.

## Разработка

Запуск тестов:

```bash
dotnet test ErrorsFlow.sln
```

Локальная упаковка NuGet-пакета без публикации:

```bash
dotnet pack ErrorsFlow.csproj --configuration Release --output artifacts
```

GitHub Actions запускает тесты перед упаковкой и публикует пакет только при push version-тега. Локальная сборка пакета не выполняет публикацию.
