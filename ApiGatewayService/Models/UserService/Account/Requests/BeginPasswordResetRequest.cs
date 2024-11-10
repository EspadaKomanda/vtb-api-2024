using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Account.Requests;

/// <summary>
/// Запрос начала операции сброса пароля.
/// </summary>
public class BeginPasswordResetRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}