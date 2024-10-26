using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models;

public class Role
{
  [Key]
  public long Id { get; set; }

  [Required]
  public string Name { get; set; } = null!;
}
