using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Account.Requests;

/// <summary>
/// Запрос на проверку кода регистрации.
/// </summary>
public class VerifyRegistrationCodeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Length(6, 6)]
    public string Code { get; set; } = null!;
}