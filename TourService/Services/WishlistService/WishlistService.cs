using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Exceptions.Database;
using TourService.Models.DTO;
using TourService.Models.Wishlist.Requests;
using UserService.Repositories;
using WishListService.Models.Wishlist.Responses;

namespace TourService.Services.WishlistService
{
    public class WishlistService(ILogger<WishlistService> logger, IUnitOfWork unitOfWork, IMapper mapper) : IWishlistService
    {
        private readonly ILogger<WishlistService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public GetWishlistedToursResponse GetWishlists(GetWishlistedToursRequest getWishlistedToursRequest)
        {
            try
            {
                var currentWishlists = _unitOfWork.TourWishes.GetAll().Where(x => x.UserId == getWishlistedToursRequest.UserId).Skip((getWishlistedToursRequest.Page - 1) * 10).Take(10);
                GetWishlistedToursResponse currentWishlist = new GetWishlistedToursResponse()
                {
                    Amount = currentWishlists.Count(),
                    Page = getWishlistedToursRequest.Page,
                    Tours = _mapper.ProjectTo<TourDto>(_unitOfWork.Tours.GetAll().Where(x => currentWishlists.Any(w => w.TourId == x.Id)).Skip((getWishlistedToursRequest.Page - 1) * 10).Take(10)).ToList()
                };
                return currentWishlist;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting wishlists: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting wishlists", ex);
            }
        }
        public async Task<bool> AddTourToWishlist(WishlistTourRequest addTourToWishlistRequest)
        {
            try
            {
                TourWish tourWish = new TourWish()
                {
                    TourId = addTourToWishlistRequest.TourId,
                    UserId = addTourToWishlistRequest.UserId,
                };
                await _unitOfWork.TourWishes.AddAsync(tourWish);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added tour to wishlist");
                    return true;
                }
                throw new DatabaseException("Error saving tour to wishlist");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding tour to wishlist: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding tour to wishlist", ex);
            }
        }

        public bool UnwishlistTour(UnwishlistTourRequest unwishTourRequest)
        {
            try
            {
                _unitOfWork.TourWishes.Delete(_unitOfWork.TourWishes.FindOneAsync(x => x.TourId == unwishTourRequest.TourId && x.UserId == unwishTourRequest.UserId).Result);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully unwishlisted tour");
                    return true;
                }
                throw new DatabaseException("Error unwishlisting tour");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error unwishlisting tour: {errorMessage}", ex.Message);
                throw new DatabaseException("Error unwishlisting tour", ex);
            }
        }
    }
}