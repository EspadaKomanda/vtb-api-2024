namespace TourService.Models.Tour.Responses;

public class GetTourResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> Categories { get; set; } = null!;
    public List<string> Tags { get; set; } = null!;
    public double Price {get; set; }
    public string Address  { get; set; } = null!;
    public string Coordinates { get; set; } = null!;
    public double? SettlementDistance {get; set; }
    public string? Comment { get; set; }
    public bool IsActive { get; set; }
    public List<string>? Photos { get; set; }
}