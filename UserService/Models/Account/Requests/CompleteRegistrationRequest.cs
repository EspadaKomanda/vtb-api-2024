using System.ComponentModel.DataAnnotations;
using UserService.Attributes.Validation;

namespace UserService.Models.Account.Requests;

/// <summary>
/// Запрос на завершение регистрации.
/// </summary>
public class CompleteRegistrationRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Length(6,6)]
    public string RegistrationCode { get; set; } = null!;

    [Required]
    [ValidName]
    public string Name { get; set; } = null!;

    [Required]
    [ValidName]
    public string Surname { get; set; } = null!;

    [ValidName]
    public string? Patronymic { get; set; }

    [Required]
    [ValidAge]
    public DateTime Birthday { get; set; }

    public string? Avatar { get; set; }
}