using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Responses;

/// <summary>
/// Результат инициализации процесса регистрации.
/// </summary>
public class RegisterResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}