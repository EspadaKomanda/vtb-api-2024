using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат запроса на сброс пароля.
/// </summary>
public class CompletePasswordResetResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}