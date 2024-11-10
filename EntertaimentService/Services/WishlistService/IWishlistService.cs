using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.DTO;
using EntertaimentService.Models.Wishlist.Requests;
using EntertaimentService.Models.Wishlist.Responses;

namespace EntertaimentService.Services.WishlistService
{
    public interface IWishlistService
    {
        GetWishlistedEntertaimentsResponse GetWishlists(GetWishlistedEntertaimentsRequest getWishlistedEntertaimentsRequest);
        Task<bool> AddEntertaimentToWishlist(WishlistEntertaimentRequest wishlistEntertaimentRequest);
        bool UnwishlistEntertaiment(UnwishlistEntertaimentRequest unwishlistEntertaimentRequest);
    }
}