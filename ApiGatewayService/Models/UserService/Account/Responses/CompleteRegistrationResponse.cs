using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Account.Responses;

/// <summary>
/// Результат завершения регистрации.
/// </summary>
public class CompleteRegistrationResponse
{
    [Required]
    public bool IsSuccess { get; set; }
}