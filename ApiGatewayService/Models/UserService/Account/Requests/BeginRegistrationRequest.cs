using System.ComponentModel.DataAnnotations;
using UserService.Attributes.Validation;

namespace ApiGatewayService.Models.UserService.Account.Requests;

/// <summary>
/// Запрос на регистрацию пользователя.
/// </summary>
public class BeginRegistrationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [ValidUsername]
    public string Username { get; set; } = null!;

    [Required]
    [ValidPassword]
    public string Password { get; set; } = null!;
}