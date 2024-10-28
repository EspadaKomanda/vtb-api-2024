using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат запроса на проверку актуальности кода регистрации.
/// </summary>
public class VerifyRegistrationCodeResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}