using EntertaimentService.Models.DTO;

namespace EntertaimentService.Models.Tour.Responses;

public class GetEntertaimentsResponse
{
    public long Amount { get; set; }
    public int Page { get; set; }
    public List<EntertaimentDto> Entertaiments { get; set; } = null!;
}