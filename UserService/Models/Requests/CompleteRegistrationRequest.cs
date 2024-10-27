using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Requests;

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
    public string Name { get; set; } = null!;

    [Required]
    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    [Required]
    // TODO: Ограничение по возрасту
    public DateTime Birthday { get; set; }

    public string? Avatar { get; set; }
}