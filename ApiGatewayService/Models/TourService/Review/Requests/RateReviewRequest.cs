using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.TourService.Models.Review.Requests;

public class RateReviewRequest
{
    [Required]
    public long ReviewId { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public bool Rating { get; set; }
}