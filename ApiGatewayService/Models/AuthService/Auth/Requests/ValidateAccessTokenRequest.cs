using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.AuthService.Authentication.Requests;

public class ValidateAccessTokenRequest
{
    [Required]
    public string AccessToken { get; set; } = null!;
}