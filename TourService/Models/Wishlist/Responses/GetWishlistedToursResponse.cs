using TourService.Models.DTO;
using TourService.Models.Tour.Responses;

namespace WishListService.Models.Wishlist.Responses;

public class GetWishlistedToursResponse
{
    public int Amount { get; set; }
    public int Page { get; set; }
    public List<TourDto> Tours { get; set; } = null!;
}
