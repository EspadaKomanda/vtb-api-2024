using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Requests;

/// <summary>
/// Запрос на проверку кода восстановления.
/// </summary>
public class VerifyResetCodeRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    // TODO: guid
    [Length(36, 36)]
    public string Code { get; set; } = null!;
}