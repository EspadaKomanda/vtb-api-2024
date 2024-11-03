using System.ComponentModel.DataAnnotations;

namespace WishListService.Models.Wishlist.Requests;

public class WishlistTourRequest
{
    [Required]
    public long TourId { get; set; }
}