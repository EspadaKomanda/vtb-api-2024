using System.ComponentModel.DataAnnotations;
using UserService.Attributes.Validation;

namespace UserService.Database.Models;

public class Meta
{
  [Key]
  public long Id { get; set; }

  [Required]
  public long UserId { get; set; }
  public virtual User User { get; set; } = null!;

  [Required]
  public string Name { get; set; } = null!;
  [Required]
  public string Surname { get; set; } = null!;
  public string? Patronymic { get; set; }

  [Required]
  [ValidAge]
  public DateTime Birthday { get; set; }

  [ValidAvatar]
  public string? Avatar { get; set; }
}
