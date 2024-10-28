using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат изменения пароля.
/// </summary>
public class ChangePasswordResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}