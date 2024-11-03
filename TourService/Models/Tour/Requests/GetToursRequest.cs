using TourService.Attributes.Validation;

namespace TourService.Models.Tour.Requests;

// TODO: finish this
public class GetToursRequest
{
    public int Page { get; set; } = 0;
    public List<string>? Categories { get; set; }
    public List<string>? TourTags { get; set; }
    [Rating]
    public int MinimalRating { get; set; } = 0;
    public double MinimalPrice { get; set; } = 0;
    public double MaximalPrice { get; set; } = double.MaxValue;
}