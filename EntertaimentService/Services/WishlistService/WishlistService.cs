using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Exceptions.Database;
using EntertaimentService.Models.DTO;
using EntertaimentService.Models.Wishlist.Requests;
using UserService.Repositories;
using EntertaimentService.Models.Wishlist.Responses;

namespace TourService.Services.WishlistService
{
    public class WishlistService(ILogger<WishlistService> logger, IUnitOfWork unitOfWork, IMapper mapper) : IWishlistService
    {
        private readonly ILogger<WishlistService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public GetWishlistedEntertaimentsResponse GetWishlists(GetWishlistedEntertaimentsRequest getWishlistedEntertaimentsRequest)
        {
            try
            {
                var currentWishlists = _unitOfWork.EntertaimentWishes.GetAll().Where(x => x.UserId == getWishlistedEntertaimentsRequest.UserId).Skip((getWishlistedEntertaimentsRequest.Page - 1) * 10).Take(10);
                GetWishlistedEntertaimentsResponse currentWishlist = new GetWishlistedEntertaimentsResponse()
                {
                    Amount = currentWishlists.Count(),
                    Page = getWishlistedEntertaimentsRequest.Page,
                    Entertaiments = _mapper.ProjectTo<EntertaimentDto>(_unitOfWork.Entertaiments.GetAll().Where(x => currentWishlists.Any(w => w.EntertaimentId == x.Id)).Skip((getWishlistedEntertaimentsRequest.Page - 1) * 10).Take(10)).ToList()
                };
                return currentWishlist;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting wishlists: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting wishlists", ex);
            }
        }
        public async Task<bool> AddEntertaimentToWishlist(WishlistEntertaimentRequest addEntertaimentToWishlistRequest)
        {
            try
            {
                EntertaimentWish entertaimentWish = new EntertaimentWish()
                {
                    EntertaimentId = addEntertaimentToWishlistRequest.EntertaimentId,
                    UserId = addEntertaimentToWishlistRequest.UserId,
                };
                await _unitOfWork.EntertaimentWishes.AddAsync(entertaimentWish);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added entertaiment to wishlist");
                    return true;
                }
                throw new DatabaseException("Error saving entertaiment to wishlist");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding entertaiment to wishlist: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding entertaiment to wishlist", ex);
            }
        }

        public bool UnwishlistEntertaiment(UnwishlistEntertaimentRequest unwishEntertaimentRequest)
        {
            try
            {
                _unitOfWork.EntertaimentWishes.Delete(_unitOfWork.EntertaimentWishes.FindOneAsync(x => x.EntertaimentId == unwishEntertaimentRequest.EntertaimentId && x.UserId == unwishEntertaimentRequest.UserId).Result);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully unwishlisted entertaiment");
                    return true;
                }
                throw new DatabaseException("Error unwishlisting entertaiment");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error unwishlisting entertaiment: {errorMessage}", ex.Message);
                throw new DatabaseException("Error unwishlisting entertaiment", ex);
            }
        }
    }
}