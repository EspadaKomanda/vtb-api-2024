using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Auth.Requests;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}