using System.ComponentModel.DataAnnotations;
using UserService.Attributes.Validation;

namespace UserService.Models.Requests;

/// <summary>
/// Запрос на изменение пароля.
/// Если LogoutSessions true, для пользователя будет сгенерирована
/// новая salt, что приведет к выходу из всех сеансов.
/// </summary>
public class ChangePasswordRequest
{
    [Required]
    public string OldPassword { get; set; } = null!;

    [Required]
    [ValidPassword]
    public string NewPassword { get; set; } = null!;

    [Required]
    public bool LogoutSessions { get; set; } = true;
}