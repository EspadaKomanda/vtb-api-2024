using EntertaimentService.Models.DTO;

namespace Entertaiment.Models.Tour.Responses;

public class GetToursResponse
{
    public long Amount { get; set; }
    public int Page { get; set; }
    public List<EntertaimentDto> Entertaiments { get; set; } = null!;
}