using ApiGatewayService.Models.TourService.Models.DTO;

namespace TourService.Models.Review.Responses;

public class GetReviewResponse
{
    public ReviewDto Review { get; set; } = null!;

}