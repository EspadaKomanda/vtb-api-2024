using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.Wishlist.Requests;
using EntertaimentService.Models.Wishlist.Responses;

namespace ApiGatewayService.Services.EntertaimentService.Wishlist
{
    public interface IEntertaimentWishlistService
    {
        Task<GetWishlistedEntertaimentsResponse> GetWishlistedEntertaiments(GetWishlistedEntertaimentsRequest getWishlistedEntertaimentsRequest);
        Task<UnwishlistEntertaimentResponse> UnwishlistEntertaiment(UnwishlistEntertaimentRequest unwishlistEntertaimentRequest);
        Task<WishlistEntertaimentResponse> WishlistEntertaiment(WishlistEntertaimentRequest wishlistEntertaimentRequest);
    }
}