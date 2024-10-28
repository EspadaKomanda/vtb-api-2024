using System.ComponentModel.DataAnnotations;
using UserService.Attributes.Validation;

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
    [ValidGuid]
    public string Code { get; set; } = null!;
}