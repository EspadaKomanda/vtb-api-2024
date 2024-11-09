using EntertaimentService.Models.DTO;

namespace EntertaimentService.Models.Wishlist.Responses;

public class GetWishlistedEntertaimentsResponse
{
    public int Amount { get; set; }
    public int Page { get; set; }
    public List<EntertaimentDto> Entertaiments { get; set; } = null!;
}
