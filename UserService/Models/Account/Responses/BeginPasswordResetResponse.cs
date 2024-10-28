using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат запроса на сброс пароля.
/// </summary>
public class BeginPasswordResetResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}