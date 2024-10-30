using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.Auth.Responses;

/// <summary>
/// RefreshResponse содержит access и refresh токены.
/// </summary>
public class RefreshResponse
{
    [Required]
    public string RefreshToken { get; set; } = null!;

    [Required]
    public string AccessToken { get; set; } = null!;
}