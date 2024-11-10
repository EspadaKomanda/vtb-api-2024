using TourService.Models.DTO;

namespace TourService.Models.Tour.Responses;

public class GetToursResponse
{
    public long Amount { get; set; }
    public int Page { get; set; }
    public List<TourDto> Tours { get; set; } = null!;
}