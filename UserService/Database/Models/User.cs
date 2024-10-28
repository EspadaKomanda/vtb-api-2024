using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Database.Models;

public class User
{
  [Key]
  public long Id { get; set; }
  [Required]
  [Column(TypeName = "VARCHAR(18)")]
  [MinLength(3)]
  [MaxLength(18)]
  public string Username { get; set; } = null!;

  [EmailAddress]
  public string Email { get; set; } = null!;

  [Phone]
  public string? Phone { get; set; }

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

  [Required]
  public bool IsActivated { get; set; } = false;

  [Required]
  public bool IsTerminated { get; set; } = false;
  public string? TerminationReason { get; set; }
  public DateTime? AllowRecreationAt { get; set; }

  [Required]
  public DateTime RegistrationDate { get; set; }

  public string? Salt;
}
