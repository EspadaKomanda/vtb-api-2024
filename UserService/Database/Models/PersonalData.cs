using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models;

public class PersonalData
{
  [Key]
  public long Id { get; set; }
  
  [Required]
  public long UserId { get; set; }
  public virtual User User { get; set; } = null!;

  public string? Passport { get; set; }
  public string? Snils { get; set; }
}
