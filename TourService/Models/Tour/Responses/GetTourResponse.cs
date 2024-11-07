using TourService.Models.DTO;

namespace TourService.Models.Tour.Responses;

public class GetTourResponse
{
    public TourDto Tour { get; set; } = null!;
}