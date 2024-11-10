using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EntertaimentService.Database.Models;

[Index(nameof(ReviewId), nameof(UserId), IsUnique = true)]
public class ReviewFeedback
{
    [Key]
    public long Id { get; set; }

    [Required]
    public long ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    [Required]
    public long UserId { get; set; }
    
    [Required]
    public bool IsPositive { get; set; }
}