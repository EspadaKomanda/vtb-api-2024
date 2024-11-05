using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Auth.Requests;

public class ValidateAccessTokenRequest
{
    [Required]
    public string AccessToken { get; set; } = null!;
}