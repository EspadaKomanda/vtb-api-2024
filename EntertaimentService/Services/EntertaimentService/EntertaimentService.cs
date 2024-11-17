using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Exceptions.Database;
using EntertaimentService.Models.Category;
using EntertaimentService.Models.DTO;
using EntertaimentService.Models.Entertaiment.Requests;
using EntertaimentService.Models.PaymentMethod;
using EntertaimentService.Models.Tour.Requests;
using TourService.Models.Category;
using TourService.Models.PaymentMethod;
using UserService.Repositories;

namespace EntertaimentService.Services.EntertaimentServices
{
    public class EntertaimentService(ILogger<EntertaimentService> logger, IUnitOfWork unitOfWork, IMapper mapper) : IEntertaimentService
    {
        private readonly ILogger<EntertaimentService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<long> AddEntertaiment(Entertaiment entertaiment)
        {
            try
            {
                await _unitOfWork.Entertaiments.AddAsync(entertaiment);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added entertaiment");
                    return entertaiment.Id;
                }
                throw new DatabaseException("Error saving entertaiment");
            }
            catch(Exception ex)
            {
                _logger.LogError("Error adding entertaiment: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding entertaiment", ex);
            }
        }
        public async Task<EntertaimentDto> GetEntertaiment(GetEntertaimentRequest getEntertaiment)
        {
            try
            {
                Entertaiment currentEntertaiment = await _unitOfWork.Entertaiments.FindOneAsync(x=>x.Id == getEntertaiment.Id);
                EntertaimentDto entertaimentDto = _mapper.Map<EntertaimentDto>(currentEntertaiment);
                entertaimentDto.Photos = _mapper.ProjectTo<PhotoDto>(_unitOfWork.Photos.GetAll().Where(x=>x.EntertaimentId == getEntertaiment.Id)).ToList();
                entertaimentDto.Reviews = _mapper.ProjectTo<ReviewDto>(_unitOfWork.Reviews.GetAll().Where(x=>x.EntertaimentId == getEntertaiment.Id)).ToList();
                entertaimentDto.Categories = _mapper.ProjectTo<CategoryDto>(_unitOfWork.EntertaimentCategories.GetAll().Where(x => x.EntertaimentId == getEntertaiment.Id).Select(x => x.Category)).ToList();
                entertaimentDto.PaymentMethods = _mapper.ProjectTo<PaymentMethodDto>(_unitOfWork.EntertaimentPayments.GetAll().Where(x => x.EntertaimentId == getEntertaiment.Id).Select(x => x.PaymentMethod)).ToList();
                return entertaimentDto;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting entertaiment: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting entertaiment", ex);
            }
        }
        public IQueryable<EntertaimentDto> GetEntertaiments(GetEntertaimentsRequest getEntertaiments)
        {
            try
            {
                IQueryable<Entertaiment> entertaiments;
                if(getEntertaiments.Categories!=null)
                {
                    entertaiments = _unitOfWork.Entertaiments.GetAll()
                    .Join(_unitOfWork.EntertaimentCategories.GetAll(), t => t.Id, tc => tc.EntertaimentId, (t, tc) => new { Entertaiment = t, Category = tc })
                    .Where(x => getEntertaiments.Categories.Contains(x.Category.CategoryId))
                    .Select(x => x.Entertaiment)
                    .Distinct()
                    .Where(x=>x.Rating>= getEntertaiments.MinimalRating && x.Rating<=getEntertaiments.MaximalRating)
                    .Where(x=>x.Price>=getEntertaiments.MinimalPrice && x.Price<=getEntertaiments.MaximalPrice)
                    .Skip((getEntertaiments.Page - 1) * 10)
                    .Take(10);
                }
                else
                {
                    entertaiments = _unitOfWork.Entertaiments.GetAll()
                    .Where(x=>x.Rating>= getEntertaiments.MinimalRating && x.Rating<=getEntertaiments.MaximalRating)
                    .Where(x=>x.Price>=getEntertaiments.MinimalPrice && x.Price<=getEntertaiments.MaximalPrice)
                    .Skip((getEntertaiments.Page - 1) * 10)
                    .Take(10);
                }
                var entertaimentDtos = _mapper.ProjectTo<EntertaimentDto>(entertaiments);
                foreach(var entertaimentDto in entertaimentDtos)
                {
                    entertaimentDto.Photos = _mapper.ProjectTo<PhotoDto>(_unitOfWork.Photos.GetAll().Where(x=>x.EntertaimentId == entertaimentDto.Id)).ToList();
                    entertaimentDto.Reviews = _mapper.ProjectTo<ReviewDto>(_unitOfWork.Reviews.GetAll().Where(x=>x.EntertaimentId == entertaimentDto.Id)).ToList();
                    entertaimentDto.Categories = _mapper.ProjectTo<CategoryDto>(_unitOfWork.EntertaimentCategories.GetAll().Where(x => x.EntertaimentId == entertaimentDto.Id).Select(x => x.Category)).ToList();
                    entertaimentDto.PaymentMethods = _mapper.ProjectTo<PaymentMethodDto>(_unitOfWork.EntertaimentPayments.GetAll().Where(x => x.EntertaimentId == entertaimentDto.Id).Select(x => x.PaymentMethod)).ToList();
                }
                _logger.LogDebug("Successefully got tours");
                return entertaimentDtos;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting tours: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting tours", ex);
            }
        }
        public bool UpdateEntertaiment(UpdateEntertaimentRequest updateEntertaiment)
        {
            try
            {
                var currentEntertaiment = _unitOfWork.Entertaiments.FindOneAsync(x => x.Id == updateEntertaiment.EntertaimentId).Result;
                currentEntertaiment.Name = updateEntertaiment.Name;
                currentEntertaiment.Description = updateEntertaiment.Description;
                currentEntertaiment.Price = updateEntertaiment.Price;
                currentEntertaiment.Address = updateEntertaiment.Address;
                if(updateEntertaiment.Coordinates!=null)
                {
                    updateEntertaiment.Coordinates = updateEntertaiment.Coordinates;
                }
                updateEntertaiment.IsActive = updateEntertaiment.IsActive;
                _unitOfWork.Entertaiments.Update(currentEntertaiment);
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
        public bool RemoveEntertaiment(RemoveEntertaimentRequest removeEntertaiment)
        {
            var transaction = _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Entertaiments.Delete(_unitOfWork.Entertaiments.FindOneAsync(x => x.Id == removeEntertaiment.EntertaimentId).Result);
                _unitOfWork.EntertaimentCategories.DeleteMany(x => x.EntertaimentId == removeEntertaiment.EntertaimentId);
                _unitOfWork.EntertaimentPayments.DeleteMany(x => x.EntertaimentId == removeEntertaiment.EntertaimentId);
                _unitOfWork.EntertaimentWishes.DeleteMany(x => x.EntertaimentId == removeEntertaiment.EntertaimentId);
                _unitOfWork.Reviews.DeleteMany(x => x.EntertaimentId == removeEntertaiment.EntertaimentId);
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
       
        public async Task<bool> LinkCategories(LinkCategoriesRequests linkCategories)
        {
            try
            {
                foreach(var categoryId in linkCategories.Categories!)
                {
                    await _unitOfWork.EntertaimentCategories.AddAsync(new EntertaimentCategory(){
                        EntertaimentId = linkCategories.EntertaimentId,
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
                    await _unitOfWork.EntertaimentPayments.AddAsync(new EntertaimentPaymentMethod(){
                        EntertaimentId = linkPaymentMethods.EntertaimentId,
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
       
        public bool UnlinkCategory(UnlinkCategoryRequest unlinkCategories)
        {
            try
            {
                _unitOfWork.EntertaimentCategories.Delete(_unitOfWork.EntertaimentCategories.FindOneAsync(x => x.EntertaimentId == unlinkCategories.EntertaimentId && x.CategoryId == unlinkCategories.CategoryId).Result);
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
                _unitOfWork.EntertaimentPayments.Delete(_unitOfWork.EntertaimentPayments.FindOneAsync(x => x.EntertaimentId == unlinkPaymentMethods.EntertaimentId && x.PaymentMethodId == unlinkPaymentMethods.PaymentMethodId).Result);
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