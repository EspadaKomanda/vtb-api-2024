using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Attributes.Validation;

namespace UserService.Database.Models;

public class User
{
  [Key]
  public long Id { get; set; }
  [Required]
  [Column(TypeName = "VARCHAR(18)")]
  [ValidUsername]
  public string Username { get; set; } = null!;

  [Required]
  [EmailAddress]
  public string Email { get; set; } = null!;

  [Phone]
  public string? Phone { get; set; }

  [Required]
  public string Password { get; set; } = null!;

  [Required]
  public virtual Role Role { get; set; } = null!;
  public long RoleId { get; set; }

  [Required]
  public virtual Meta Meta { get; set; } = null!;
  public long MetaId { get; set; }

  [Required]
  public virtual PersonalData PersonalData { get; set; } = null!;
  public long PersonalDataId { get; set; }

  [Required]
  public bool IsActivated { get; set; } = false;

  [Required]
  public bool IsTerminated { get; set; } = false;
  public string? TerminationReason { get; set; }
  public DateTime? AllowRecreationAt { get; set; }

  [Required]
  public DateTime RegistrationDate { get; set; }

  [Required]
  public string Salt { get; set; } = null!;
}
