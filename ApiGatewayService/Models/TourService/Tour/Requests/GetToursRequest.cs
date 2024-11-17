namespace TourService.Models.Tour.Requests;

// TODO: finish this
public class GetToursRequest
{
    public int Page { get; set; } = 0;
    public List<long>? Categories { get; set; }
    public List<long>? TourTags { get; set; }
    public int MinimalRating { get; set; } = 0;
    public int MaximalRating { get; set; } = 5;
    public double MinimalPrice { get; set; } = 0;
    public double MaximalPrice { get; set; } = double.MaxValue;
}