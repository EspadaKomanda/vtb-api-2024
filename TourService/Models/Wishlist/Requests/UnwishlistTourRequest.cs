using System.ComponentModel.DataAnnotations;

namespace WishListService.Models.Wishlist.Requests;

public class UnwishlistTourRequest
{
    [Required]
    public long TourId { get; set; }
}