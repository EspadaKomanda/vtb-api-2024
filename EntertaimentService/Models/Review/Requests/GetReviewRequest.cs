using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Review.Requests;

public class GetReviewRequest
{
    [Required]
    public long ReviewId { get; set; }
}