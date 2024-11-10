using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Authentication.Requests;

public class ValidateAccessTokenRequest
{
    [Required]
    public string AccessToken { get; set; } = null!;
}