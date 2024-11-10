using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат завершения регистрации.
/// </summary>
public class CompleteRegistrationResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}