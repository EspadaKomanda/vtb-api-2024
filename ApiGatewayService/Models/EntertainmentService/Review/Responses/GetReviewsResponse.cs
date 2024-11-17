using EntertaimentService.Models.DTO;

namespace EntertaimentService.Models.Review.Responses;

public class GetReviewsResponse
{
    public long Amount { get; set; }
    public int Page { get; set; }
    public List<ReviewEntertaimentDto>? Reviews { get; set; }
}