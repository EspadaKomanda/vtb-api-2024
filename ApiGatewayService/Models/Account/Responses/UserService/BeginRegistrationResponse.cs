using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат инициализации процесса регистрации.
/// </summary>
public class BeginRegistrationResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}