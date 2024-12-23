using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Database.Models;

[Index(nameof(UserId), IsUnique = true)]
public class RegistrationCode
{
    [Key]
    public long Id { get; set; }

    // FIXME: Match the six-character registration code template
    [Required]
    [Column(TypeName = "VARCHAR(6)")]
    public string Code { get; set; } = Guid.NewGuid().ToString()[..6];

    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddMinutes(10);

    [Required]
    public User User { get; set; } = null!;
    public long UserId { get; set; }
}