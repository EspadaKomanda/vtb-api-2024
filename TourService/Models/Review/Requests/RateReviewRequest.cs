using System.ComponentModel.DataAnnotations;

namespace TourService.Models.Review.Requests;

public class RateReviewRequest
{
    [Required]
    public long ReviewId { get; set; }

    [Required]
    public bool IsPositive { get; set; }
}