using System.ComponentModel.DataAnnotations;

namespace EntertaimentService.Models.Wishlist.Requests;

public class GetWishlistedEntertaimentsRequest
{
    [Required]
    public long UserId { get; set; }
    [Required]
    public int Page = 0;
}