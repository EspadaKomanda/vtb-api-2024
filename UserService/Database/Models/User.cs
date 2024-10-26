using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models;

public class User
{
  [Key]
  public long Id { get; set; }
  [Required]
  public string Username { get; set; } = null!;
  [Required]
  public string Password { get; set; } = null!;

  [Required]
  public Role Role { get; set; } = null!;
  public long RoleId { get; set; }

  [Required]
  public Meta Meta { get; set; } = null!;
  public long MetaId { get; set; }

  [Required]
  public PersonalData PersonalData { get; set; } = null!;
  public long PersonalDataId { get; set; }

  public string? Salt;
}
