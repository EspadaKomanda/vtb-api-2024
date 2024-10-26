using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models;

public class Meta
{
  [Key]
  public long Id { get; set; }

  [Required]
  public long UserId { get; set; }
  public User User { get; set; } = null!;

  [Required]
  public string Name { get; set; } = null!;
  [Required]
  public string Surname { get; set; } = null!;
  [Required]
  public string Patronymic { get; set; } = null!;

  [Required]
  public DateTime Birthday { get; set; }

  public string? Avatar { get; set; }
}
