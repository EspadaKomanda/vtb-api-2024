using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Account.Responses;

/// <summary>
/// Результат запроса на повторное отправку кода для сброса пароля.
/// </summary>
public class ResendPasswordResetCodeResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}