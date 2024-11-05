using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models.Account.Requests;
using UserService.Models.Account.Responses;

namespace ApiGatewayService.Services.User
{
    /// <summary>
    /// <c>IAccountService</c> ответственен за работу с аккаунтом 
    /// пользователя.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Выполняет регистрацию нового пользователя и отправляет 
        /// код подтверждения на почту.
        /// </summary>
        Task<BeginRegistrationResponse> BeginRegistration(BeginRegistrationRequest request);

        /// <summary>
        /// Позволяет завершить регистрацию пользователя и активировать 
        /// его аккаунт.
        /// </summary>
        Task<CompleteRegistrationResponse> CompleteRegistration(CompleteRegistrationRequest request);

        /// <summary>
        /// Позволяет проверить, является ли код подтверждения для
        /// регистрации пользователя актуальным.
        /// </summary>
        Task<VerifyRegistrationCodeResponse> VerifyRegistrationCode(VerifyRegistrationCodeRequest request);

        /// <summary>
        /// Позволяет переотправить код подтверждения, если он был 
        /// утерян или его срок действия истек.
        /// </summary>
        Task<ResendRegistrationCodeResponse> ResendRegistrationCode(ResendRegistrationCodeRequest request);

        /// <summary>
        /// Позволяет начать сброс пароля пользователя. Отправляет 
        /// код сброса пароля на почту пользователя.
        /// </summary>
        Task<BeginPasswordResetResponse> BeginPasswordReset(BeginPasswordResetRequest request);

        /// <summary>
        /// Позволяет завершить сброс пароля пользователя.
        /// </summary>
        Task<CompletePasswordResetResponse> CompletePasswordReset(CompletePasswordResetRequest request);

        /// <summary>
        /// Позволяет проверить, является ли код сброса пароля актуальным.
        /// </summary>
        Task<VerifyPasswordResetCodeResponse> VerifyPasswordResetCode(VerifyPasswordResetCodeRequest request);

        /// <summary>
        /// Позволяет переотправить код сброса пароля, если он был утерян 
        /// или его срок действия истек.
        /// </summary>
        Task<ResendPasswordResetCodeResponse> ResendPasswordResetCode(ResendPasswordResetCodeRequest request);

        /// <summary>
        /// Позволяет изменить пароль пользователя. Отправляет сообщение 
        /// об изменении пароля на почту пользователя, а также
        /// оповещает сервисы AuthService и ApiGateway об изменении.
        /// </summary>
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request);

        /// <summary>
        /// Позволяет получить данные для входа в аккаунт. Предназначен 
        /// только для использования в микросервисе AuthService.
        /// </summary>
        Task<AccountAccessDataResponse> AccountAccessData(AccountAccessDataRequest request);
    }
}