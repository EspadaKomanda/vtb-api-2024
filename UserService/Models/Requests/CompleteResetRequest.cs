using System.ComponentModel.DataAnnotations;
using UserService.Attributes.Validation;

namespace UserService.Models.Requests;

/// <summary>
/// Запрос на завершение сброса пароля.
/// </summary>
public class CompleteResetRequest
{
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [ValidGuid]
    public string Code { get; set; } = null!;

    [Required]
    [ValidPassword]
    public string NewPassword = null!;
}