using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Responses;

/// <summary>
/// Результат запроса на проверку актуальности кода восстановления.
/// </summary>
public class VerifyResetCodeResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}