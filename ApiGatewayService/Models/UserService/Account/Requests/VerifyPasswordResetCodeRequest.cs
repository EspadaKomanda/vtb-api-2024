using System.ComponentModel.DataAnnotations;
using UserService.Attributes.Validation;

namespace ApiGatewayService.Models.UserService.Account.Requests;

/// <summary>
/// Запрос на проверку кода восстановления.
/// </summary>
public class VerifyPasswordResetCodeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [ValidGuid]
    public string Code { get; set; } = null!;
}