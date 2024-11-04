using System.ComponentModel.DataAnnotations;

namespace TourService.Models.Wishlist.Requests;

public class UnwishlistTourRequest
{
    [Required]
    public long TourId { get; set; }
}