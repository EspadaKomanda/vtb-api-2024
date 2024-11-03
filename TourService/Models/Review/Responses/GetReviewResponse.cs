namespace TourService.Models.Review.Responses;

public class GetReviewResponse
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatar { get; set; }
    public string? Text { get; set; }
    public List<string>? Benefits { get; set; }
    public int Rating { get; set; }
    public int PositiveFeedbacksCount { get; set; }
    public int NegativeFeedbacksCount { get; set; }

}