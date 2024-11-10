using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Account.Requests;

/// <summary>
/// Запрос на повторное отправку кода регистрации.
/// </summary>
public class ResendRegistrationCodeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}