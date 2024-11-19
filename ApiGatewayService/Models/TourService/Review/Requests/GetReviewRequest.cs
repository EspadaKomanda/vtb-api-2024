using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.TourService.Models.Review.Requests;

public class GetReviewRequest
{
    [Required]
    public long ReviewId { get; set; }
}