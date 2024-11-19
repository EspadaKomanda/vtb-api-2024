using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Models.Wishlist.Requests;
using TourService.Models.Wishlist.Responses;
using WishListService.Models.Wishlist.Responses;

namespace ApiGatewayService.Services.TourService.Wishlist
{
    public interface ITourWishlistService
    {
        Task<GetWishlistedToursResponse> GetWishlistedTours(GetWishlistedToursRequest getWishlistedToursRequest);
        Task<UnwishlistTourResponse> UnwishlistTour(UnwishlistTourRequest unwishlistTourRequest);
        Task<WishlistTourResponse> WishlistTour(WishlistTourRequest wishlistTourRequest);
    }
}