using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Responses;

/// <summary>
/// Данные для доступа к аккаунту.
/// </summary>
public class AccountAccessDataResponse
{
    [Required]
    public long UserId { get; set; }

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Salt { get; set; } = null!;
}