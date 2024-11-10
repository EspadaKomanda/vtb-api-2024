using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Account.Responses;

/// <summary>
/// Результат изменения пароля.
/// </summary>
public class ChangePasswordResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}