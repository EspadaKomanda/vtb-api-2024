using System.ComponentModel.DataAnnotations;

namespace TourService.Models.Wishlist.Requests;

public class WishlistTourRequest
{
    [Required]
    public long UserId { get; set; }
    [Required]
    public long TourId { get; set; }
}