using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Requests;

/// <summary>
/// Запрос на завершение сброса пароля.
/// </summary>
public class CompleteResetRequest
{
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Length(36, 36)]
    // TODO: guid
    public string Code { get; set; } = null!;

    [Required]
    // TODO: validation
    public string NewPassword = null!;
}