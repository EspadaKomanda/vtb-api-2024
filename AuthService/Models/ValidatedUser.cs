using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class ValidatedUser
{
    [Required]
    public long Id { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!;
}
