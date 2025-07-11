namespace Organetto.UseCases.Shared.Exceptions.Models
{
    /// <summary>
    /// Перечень возможных кодов ошибок, возвращаемых AvisAuthServer.
    /// </summary>
    public enum AppErrorCode
    {
        // Общие ошибки
        UNKNOWN_ERROR,
        DATABASE_CONNECTION_FAILED,
        VALIDATION_FAILED,

        // OAuth 2.0 / OpenID Connect
        INVALID_REQUEST,
        INVALID_CLIENT,
        INVALID_GRANT,
        UNAUTHORIZED_CLIENT,
        UNSUPPORTED_GRANT_TYPE,
        INVALID_SCOPE,

        // Токены
        INVALID_TOKEN,
        EXPIRED_TOKEN,
        INSUFFICIENT_SCOPE,
        TOKEN_REVOKED,

        // Авторизационный код
        AUTHORIZATION_CODE_INVALID,
        AUTHORIZATION_CODE_EXPIRED,
        REDIRECT_URI_MISMATCH,

        // Пользователь
        USER_NOT_FOUND,
        EMAIL_ALREADY_REGISTERED,
        EMAIL_NOT_CONFIRMED,
        INVALID_PASSWORD,
        PASSWORD_RESET_TOKEN_INVALID,

        // Клиент
        CLIENT_NOT_FOUND,
        CLIENT_DISABLED,

        // CORS
        ORIGIN_NOT_ALLOWED,

        // Ограничение частоты запросов
        RATE_LIMIT_EXCEEDED,

        // Внешние провайдеры (например, LDAP, OAuth внешнего IdP)
        EXTERNAL_PROVIDER_ERROR,

        // Сущности
        ENTITY_NOT_FOUND,
    }

}
