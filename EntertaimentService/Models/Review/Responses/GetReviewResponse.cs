using EntertaimentService.Models.DTO;
using TourService.Models.DTO;

namespace TourService.Models.Review.Responses;

public class GetReviewResponse
{
    public ReviewDto Review { get; set; } = null!;

}