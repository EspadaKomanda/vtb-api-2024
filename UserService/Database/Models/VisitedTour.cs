using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models;

public class VisitedTour
{
  [Key]
  public long Id { get; set; }

  [Required]
  public User User { get; set; } = null!;
  public long UserId { get; set; }

  [Required]
  public long TourId { get; set; }
}
