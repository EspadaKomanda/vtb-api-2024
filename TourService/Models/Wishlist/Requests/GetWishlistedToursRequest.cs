using System.ComponentModel.DataAnnotations;

namespace TourService.Models.Wishlist.Requests;

public class GetWishlistedToursRequest
{
    [Required]
    public long UserId { get; set; }
    [Required]
    public int Page = 0;
}