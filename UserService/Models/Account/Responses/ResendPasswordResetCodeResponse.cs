using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат запроса на повторное отправку кода для сброса пароля.
/// </summary>
public class ResendPasswordResetCodeResponse
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}