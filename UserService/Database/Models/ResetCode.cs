using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UserService.Attributes.Validation;

namespace UserService.Database.Models;

/// <summary>
/// Код для сброса пароля.
/// </summary>
[Index(nameof(UserId), IsUnique = true)]
public class ResetCode
{
    [Key]
    public long Id { get; set; }

    [Required]
    [ValidGuid]
    [Column(TypeName = "VARCHAR(36)")]
    public string Code { get; set; } = Guid.NewGuid().ToString();

    public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddMinutes(10);

    [Required]
    public long UserId { get; set; }
    public virtual User User { get; set; } = null!;
}