using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Requests;

/// <summary>
/// Запрос на повторное отправку кода для сброса пароля.
/// </summary>
public class ResendPasswordResetCodeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}