using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Review.Requests;

public class RateReviewEntertainmentRequest
{
    [Required]
    public long ReviewId { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public bool Rating { get; set; }
}