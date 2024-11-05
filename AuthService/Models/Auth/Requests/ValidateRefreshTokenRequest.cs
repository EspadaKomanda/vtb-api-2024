using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Auth.Requests;

public class ValidateRefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}