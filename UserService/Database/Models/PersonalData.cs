using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models;

public class PersonalData
{
  [Key]
  public long Id { get; set; }
  
  [Required]
  public User User { get; set; } = null!;
  public long UserId { get; set; }

  public string? Passport { get; set; }
  public string? Snils { get; set; }
}
