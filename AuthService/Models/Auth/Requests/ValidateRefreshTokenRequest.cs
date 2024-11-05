using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Authentication.Requests;

public class ValidateRefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}