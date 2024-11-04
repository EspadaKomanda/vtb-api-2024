using System.ComponentModel.DataAnnotations;

namespace TourService.Models.Review.Requests;

public class GetReviewRequest
{
    [Required]
    public long ReviewId { get; set; }
}