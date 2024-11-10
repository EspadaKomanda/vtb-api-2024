using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.AuthService.Authentication.Requests;

public class ValidateRefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}