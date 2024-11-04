namespace TourService.Models.Tour.Responses;

public class GetToursResponse
{
    public long Amount { get; set; }
    public int Page { get; set; }
    public List<GetTourResponse> Tours { get; set; } = null!;
}