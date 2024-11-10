using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Exceptions.Database;
using TourService.Models.Category;
using TourService.Models.DTO;
using TourService.Models.PaymentMethod;
using TourService.Models.Tour.Requests;
using UserService.Repositories;

namespace TourService.Services.TourService
{
    public class TourService(ILogger<TourService> logger, IUnitOfWork unitOfWork, IMapper mapper) : ITourService
    {
        private readonly ILogger<TourService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<long> AddTour(Tour tour)
        {
            try
            {
                await _unitOfWork.Tours.AddAsync(tour);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added tour");
                    return tour.Id;
                }
                throw new DatabaseException("Error saving tour");
            }
            catch(Exception ex)
            {
                _logger.LogError("Error adding tour: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding tour", ex);
            }
        }
        public async Task<TourDto> GetTour(GetTourRequest getTour)
        {
            try
            {
                Tour currentTour = await _unitOfWork.Tours.FindOneAsync(x=>x.Id == getTour.Id);
                TourDto tourDto = _mapper.Map<TourDto>(currentTour);
                tourDto.Photos = _mapper.ProjectTo<PhotoDto>(_unitOfWork.Photos.GetAll().Where(x=>x.TourId == getTour.Id)).ToList();
                tourDto.Reviews = _mapper.ProjectTo<ReviewDto>(_unitOfWork.Reviews.GetAll().Where(x=>x.TourId == getTour.Id)).ToList();
                tourDto.Categories = _mapper.ProjectTo<CategoryDto>(_unitOfWork.TourCategories.GetAll().Where(x => x.TourId == getTour.Id).Select(x => x.Category)).ToList();
                tourDto.Tags = _mapper.ProjectTo<TagDto>(_unitOfWork.TourTags.GetAll().Where(x => x.TourId == getTour.Id).Select(x => x.Tag)).ToList();
                tourDto.PaymentMethods = _mapper.ProjectTo<PaymentMethodDto>(_unitOfWork.TourPayments.GetAll().Where(x => x.TourId == getTour.Id).Select(x => x.PaymentMethod)).ToList();
                return tourDto;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting tour: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting tour", ex);
            }
        }
        public IQueryable<TourDto> GetTours(GetToursRequest getTours)
        {
            try
            {
                IQueryable<Tour> tours;
                if( getTours.Categories!=null && getTours.TourTags!=null)
                {
                    tours = _unitOfWork.Tours.GetAll()
                    .Join(_unitOfWork.TourCategories.GetAll(), t => t.Id, tc => tc.TourId, (t, tc) => new { Tour = t, Category = tc })
                    .Where(x => getTours.Categories.Contains(x.Category.CategoryId))
                    .Join(_unitOfWork.TourTags.GetAll(), x => x.Tour.Id, tt => tt.TourId, (x, tt) => new { Tour = x.Tour, Tag = tt })
                    .Where(x => getTours.TourTags.Contains(x.Tag.TagId))
                    .Select(x => x.Tour)
                    .Distinct()
                    .Where(x=>x.Rating>= getTours.MinimalRating && x.Rating<=getTours.MaximalRating)
                    .Where(x=>x.Price>=getTours.MinimalPrice && x.Price<=getTours.MaximalPrice)
                    .Skip((getTours.Page - 1) * 10)
                    .Take(10);
                }
                else if(getTours.Categories!=null)
                {
                    tours = _unitOfWork.Tours.GetAll()
                    .Join(_unitOfWork.TourCategories.GetAll(), t => t.Id, tc => tc.TourId, (t, tc) => new { Tour = t, Category = tc })
                    .Where(x => getTours.Categories.Contains(x.Category.CategoryId))
                    .Select(x => x.Tour)
                    .Distinct()
                    .Where(x=>x.Rating>= getTours.MinimalRating && x.Rating<=getTours.MaximalRating)
                    .Where(x=>x.Price>=getTours.MinimalPrice && x.Price<=getTours.MaximalPrice)
                    .Skip((getTours.Page - 1) * 10)
                    .Take(10);
                }
                else if(getTours.TourTags!=null)
                {
                    tours = _unitOfWork.Tours.GetAll()
                    .Join(_unitOfWork.TourTags.GetAll(), t => t.Id, tt => tt.TourId, (t, tt) => new { Tour = t, Tag = tt })
                    .Where(x => getTours.TourTags.Contains(x.Tag.TagId))
                    .Select(x => x.Tour)
                    .Distinct()
                    .Where(x=>x.Rating>= getTours.MinimalRating && x.Rating<=getTours.MaximalRating)
                    .Where(x=>x.Price>=getTours.MinimalPrice && x.Price<=getTours.MaximalPrice)
                    .Skip((getTours.Page - 1) * 10)
                    .Take(10);
                }
                else
                {
                    tours = _unitOfWork.Tours.GetAll()
                    .Where(x=>x.Rating>= getTours.MinimalRating && x.Rating<=getTours.MaximalRating)
                    .Where(x=>x.Price>=getTours.MinimalPrice && x.Price<=getTours.MaximalPrice)
                    .Skip((getTours.Page - 1) * 10)
                    .Take(10);
                }
                var tourDtos = _mapper.ProjectTo<TourDto>(tours);
                foreach(var tourDto in tourDtos)
                {
                    tourDto.Photos = _mapper.ProjectTo<PhotoDto>(_unitOfWork.Photos.GetAll().Where(x=>x.TourId == tourDto.Id)).ToList();
                    tourDto.Reviews = _mapper.ProjectTo<ReviewDto>(_unitOfWork.Reviews.GetAll().Where(x=>x.TourId == tourDto.Id)).ToList();
                    tourDto.Categories = _mapper.ProjectTo<CategoryDto>(_unitOfWork.TourCategories.GetAll().Where(x => x.TourId == tourDto.Id).Select(x => x.Category)).ToList();
                    tourDto.Tags = _mapper.ProjectTo<TagDto>(_unitOfWork.TourTags.GetAll().Where(x => x.TourId == tourDto.Id).Select(x => x.Tag)).ToList();
                    tourDto.PaymentMethods = _mapper.ProjectTo<PaymentMethodDto>(_unitOfWork.TourPayments.GetAll().Where(x => x.TourId == tourDto.Id).Select(x => x.PaymentMethod)).ToList();
                }
                _logger.LogDebug("Successefully got tours");
                return tourDtos;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting tours: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting tours", ex);
            }
        }
        public bool UpdateTour(UpdateTourRequest updateTour)
        {
            try
            {
                var currentTour = _unitOfWork.Tours.FindOneAsync(x => x.Id == updateTour.TourId).Result;
                currentTour.Name = updateTour.Name;
                currentTour.Description = updateTour.Description;
                currentTour.Price = updateTour.Price;
                currentTour.Address = updateTour.Address;
                if(updateTour.Coordinates!=null)
                {
                    currentTour.Coordinates = updateTour.Coordinates;
                }
                currentTour.IsActive = updateTour.IsActive;
                _unitOfWork.Tours.Update(currentTour);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully updated tour");
                    return true;
                }
                throw new DatabaseException("Error updating tour");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating tour: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating tour", ex);
            }
        }
        public bool RemoveTour(RemoveTourRequest removeTour)
        {
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Tours.Delete(_unitOfWork.Tours.FindOneAsync(x => x.Id == removeTour.TourId).Result);
                _unitOfWork.TourCategories.DeleteMany(x => x.TourId == removeTour.TourId);
                _unitOfWork.TourTags.DeleteMany(x => x.TourId == removeTour.TourId);
                _unitOfWork.TourPayments.DeleteMany(x => x.TourId == removeTour.TourId);
                _unitOfWork.TourWishes.DeleteMany(x => x.TourId == removeTour.TourId);
                _unitOfWork.Reviews.DeleteMany(x => x.TourId == removeTour.TourId);
                if(transaction.SaveAndCommit())
                {

                    _logger.LogDebug("Successefully removed tour");
                    return true;
                }
                throw new DatabaseException("Error removing tour");

            }
            catch(Exception ex)
            {
                transaction.Rollback();
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error removing tour: {errorMessage}", ex.Message);
                throw new DatabaseException("Error removing tour", ex);
            }
        }
        public async Task<bool> LinkTags(LinkTagsRequest linkTags)
        {
            try
            {
                foreach(var tagId in linkTags.Tags!)
                {
                    await _unitOfWork.TourTags.AddAsync(new TourTag(){
                        TourId = linkTags.TourId,
                        TagId = tagId
                    });
                }
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully linked tags");
                    return true;
                }
                throw new DatabaseException("Error linking tags");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error linking tags: {errorMessage}", ex.Message);
                throw new DatabaseException("Error linking tags", ex);
            }
        }
        public async Task<bool> LinkCategories(LinkCategoriesRequests linkCategories)
        {
            try
            {
                foreach(var categoryId in linkCategories.Categories!)
                {
                    await _unitOfWork.TourCategories.AddAsync(new TourCategory(){
                        TourId = linkCategories.TourId,
                        CategoryId = categoryId
                    });
                }
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully linked categories");
                    return true;
                }
                throw new DatabaseException("Error linking categories");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error linking categories: {errorMessage}", ex.Message);
                throw new DatabaseException("Error linking categories", ex);
            }
        }
        public async Task<bool> LinkPaymentMethods(LinkPaymentMethodsRequest linkPaymentMethods)
        {
            try
            {
                foreach(var paymentMethodId in linkPaymentMethods.PaymentMethods!)
                {
                    await _unitOfWork.TourPayments.AddAsync(new TourPaymentMethod(){
                        TourId = linkPaymentMethods.TourId,
                        PaymentMethodId = paymentMethodId
                    });
                }
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully linked payment methods");
                    return true;
                }
                throw new DatabaseException("Error linking payment methods");
            }
            catch(Exception ex)
            {
            
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error linking payment methods: {errorMessage}", ex.Message);
                throw new DatabaseException("Error linking payment methods", ex);
            }
        }
        public bool UnlinkTag(UnlinkTagRequest unlinkTags)
        {
            try
            {
                _unitOfWork.TourTags.Delete(_unitOfWork.TourTags.FindOneAsync(x => x.TourId == unlinkTags.TourId && x.TagId == unlinkTags.TagId).Result);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully unlinked tags");
                    return true;
                }
                throw new DatabaseException("Error unlinking tags");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error unlinking tags: {errorMessage}", ex.Message);
                throw new DatabaseException("Error unlinking tags", ex);
            }
        }
        public bool UnlinkCategory(UnlinkCategoryRequest unlinkCategories)
        {
            try
            {
                _unitOfWork.TourCategories.Delete(_unitOfWork.TourCategories.FindOneAsync(x => x.TourId == unlinkCategories.TourId && x.CategoryId == unlinkCategories.CategoryId).Result);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully unlinked categories");
                    return true;
                }
                throw new DatabaseException("Error unlinking categories");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error unlinking categories: {errorMessage}", ex.Message);
                throw new DatabaseException("Error unlinking categories", ex);
            }
        }
        public bool UnlinkPaymentMethod(UnlinkPaymentMethodRequest unlinkPaymentMethods)
        {
            try
            {
                _unitOfWork.TourPayments.Delete(_unitOfWork.TourPayments.FindOneAsync(x => x.TourId == unlinkPaymentMethods.TourId && x.PaymentMethodId == unlinkPaymentMethods.PaymentMethodId).Result);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully unlinked payment methods");
                    return true;
                }
                throw new DatabaseException("Error unlinking payment methods");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error unlinking payment methods: {errorMessage}", ex.Message);
                throw new DatabaseException("Error unlinking payment methods", ex);
            }
        }
    }
}