using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models;

public class VisitedTour
{
  [Key]
  public long Id { get; set; }

  [Required]
  public long UserId { get; set; }
  public virtual User User { get; set; } = null!;

  [Required]
  public long TourId { get; set; }
}
