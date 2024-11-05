using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Account.Responses;

/// <summary>
/// Результат запроса на проверку актуальности кода восстановления.
/// </summary>
public class VerifyPasswordResetCodeResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}