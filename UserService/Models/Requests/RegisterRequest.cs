using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Requests;

/// <summary>
/// Запрос на регистрацию пользователя.
/// </summary>
public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    // TODO: Add validation
    [Required]
    public string Login { get; set; } = null!;

    // TODO: Add validation
    [Required]
    public string Password { get; set; } = null!;
}