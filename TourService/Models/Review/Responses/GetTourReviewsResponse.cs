namespace TourService.Models.Review.Responses;

public class GetTourReviewsResponse
{
    public long Amount { get; set; }
    public int Page { get; set; }
    public List<GetReviewResponse>? Reviews { get; set; }
}