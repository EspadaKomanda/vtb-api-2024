using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Review.Requests;

public class GetReviewEntertainmentRequest
{
    [Required]
    public long ReviewId { get; set; }
}