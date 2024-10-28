using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Responses;

/// <summary>
/// Результат запроса на сброс пароля.
/// </summary>
public class CompleteResetResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}