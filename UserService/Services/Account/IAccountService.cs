namespace UserService.Services.Account;

/// <summary>
/// <c>IAccountService</c> ответственен за работу с аккаунтом 
/// пользователя.
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Выполняет регистрацию нового пользователя и отправляет 
    /// код подтверждения на почту.
    /// </summary>
    Task<bool> Register();

    /// <summary>
    /// Позволяет проверить, является ли код подтверждения для
    /// регистрации пользователя актуальным.
    /// </summary>
    Task<bool> VerifyRegistrationCode();

    /// <summary>
    /// Позволяет переотправить код подтверждения, если он был 
    /// утерян или его срок действия истек.
    /// </summary>
    Task<bool> ResendRegistrationCode();

    /// <summary>
    /// Позволяет завершить регистрацию пользователя и активировать 
    /// его аккаунт.
    /// </summary>
    Task<bool> CompleteRegistration();

    /// <summary>
    /// Позволяет получить данные для входа в аккаунт. Предназначен 
    /// только для использования в микросервисе AuthService.
    /// </summary>
    Task<bool> AccountAccessData();

    /// <summary>
    /// Позволяет изменить пароль пользователя. Отправляет сообщение 
    /// об изменении пароля на почту пользователя, а также
    /// оповещает сервисы AuthService и ApiGateway об изменении.
    /// </summary>
    Task<bool> ChangePassword();
}