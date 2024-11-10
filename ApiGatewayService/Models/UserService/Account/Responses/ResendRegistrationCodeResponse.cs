using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Account.Responses;

/// <summary>
/// Результат запроса на повторную отправку кода регистрации.
/// </summary>
public class ResendRegistrationCodeResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}