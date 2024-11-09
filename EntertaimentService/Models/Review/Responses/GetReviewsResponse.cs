using TourService.Models.DTO;

namespace TourService.Models.Review.Responses;

public class GetReviewsResponse
{
    public long Amount { get; set; }
    public int Page { get; set; }
    public List<ReviewDto>? Reviews { get; set; }
}