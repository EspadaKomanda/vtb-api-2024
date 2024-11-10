using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Models.DTO;
using TourService.Models.Wishlist.Requests;
using WishListService.Models.Wishlist.Responses;

namespace TourService.Services.WishlistService
{
    public interface IWishlistService
    {
        GetWishlistedToursResponse GetWishlists(GetWishlistedToursRequest getWishlistedToursRequest);
        Task<bool> AddTourToWishlist(WishlistTourRequest wishlistTourRequest);
        bool UnwishlistTour(UnwishlistTourRequest unwishlistTourRequest);
    }
}