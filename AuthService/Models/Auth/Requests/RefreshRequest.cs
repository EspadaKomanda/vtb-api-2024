using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Authentication.Requests;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}