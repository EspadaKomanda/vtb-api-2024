using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Responses;

/// <summary>
/// Результат изменения пароля.
/// </summary>
public class ChangePasswordResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}