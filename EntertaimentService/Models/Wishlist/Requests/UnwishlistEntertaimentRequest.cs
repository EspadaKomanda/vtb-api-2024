using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Wishlist.Requests;

public class UnwishlistEntertaimentRequest
{
    [Required]
    public long UserId { get; set; }
    [Required]
    public long EntertaimentId { get; set; }
}